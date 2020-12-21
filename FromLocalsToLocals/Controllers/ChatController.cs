using FromLocalsToLocals.Contracts.DTO;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Services.EF;
using FromLocalsToLocals.Web.Utilities;
using FromLocalsToLocals.Web.Utilities.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Web.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IVendorService _vendorService;
        private readonly IChatService _chatService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly IWebApiClient _webApiClient;

        public ChatController(IChatService chatService,IVendorService vendorService ,UserManager<AppUser> userManager,
            IHubContext<MessageHub> hubContext, IWebApiClient webApiClient)
        {
            _chatService = chatService;
            _vendorService = vendorService;
            _userManager = userManager;
            _hubContext = hubContext;
            _webApiClient = webApiClient;
        }

        public async Task<IActionResult> Index(string tabName, int vendorId)
        {
            var user = await _userManager.GetUserAsync(User);
            var vendor = await _vendorService.GetVendorAsync(vendorId);

            //User cannot chat with vendors that belongs to him
            if (vendorId != 0 && vendor != null && !user.Vendors.Any(x=> x.ID == vendorId))
            {
                var contact = user.Contacts.FirstOrDefault(x => x.ReceiverID == vendorId);
                if(contact != null)
                {
                    return View(Tuple.Create(user, true, contact));
                }

                contact = new Contact(user,vendor,true,false);

                await _chatService.CreateContact(contact);

                return View(Tuple.Create(user, true, contact));   
            }

            return View(Tuple.Create(user,tabName == "ISent",new Contact { ID = -1 }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] IncomingMessageDTO messageDto)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var client = _webApiClient.GetClient(user);

                var response = await client.PostAsync("/api/Chat/CreateMessage", 
                    new StringContent(JsonConvert.SerializeObject(messageDto), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var responseMessage = await response.Content.ReadAsStringAsync();
                    var outGoingMessageDto = JsonConvert.DeserializeObject<OutGoingMessageDTO>(responseMessage);
                    await _hubContext.Clients.User(outGoingMessageDto.UserToSendId).SendAsync("sendNewMessage", responseMessage);
                }

            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }


        [HttpPost]
        public async Task ReadMessage([FromBody] ContactIdDTO contactDto)
        {
            var user = await _userManager.GetUserAsync(User);

            var client = _webApiClient.GetClient(user);

            await client.PostAsync("/api/Chat/ReadMessage",
                new StringContent(JsonConvert.SerializeObject(contactDto), Encoding.UTF8, "application/json"));
        }

        public async Task<IActionResult> GetChatComponent(int contactId, bool isUserTab, string componentName)
        {
            var user = await _userManager.GetUserAsync(User);
            Contact uContact = null;
            if (isUserTab)
            {
                uContact = user.Contacts.FirstOrDefault(x => x.ID == contactId);
            }
            else
            {
                user.Vendors.ToList().ForEach(x => {
                    var c =  x.Contacts.FirstOrDefault(y => y.ID == contactId);
                    if(c != null)
                    {
                        uContact = c;
                        return;
                    }

                });
            }
            if (uContact == null)
            {
                return BadRequest();
            }

            return ViewComponent(componentName, new { contact = uContact, isUserTab });
        }
  
        
    
    }
}
