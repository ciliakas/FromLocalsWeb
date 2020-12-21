using FromLocalsToLocals.Contracts.DTO;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Services.EF;
using FromLocalsToLocals.Web.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<MessageHub> _hubContext;


        public ChatController(IChatService chatService, UserManager<AppUser> userManager,IHubContext<MessageHub> hubContext)
        {
            _chatService = chatService;
            _userManager = userManager;
            _hubContext = hubContext;

        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateMessage([FromBody] IncomingMessageDTO incMessageDto)
        {
            OutGoingMessageDTO outgoingMessageDto;
            try
            {
                var user = await _userManager.GetUserAsync(User);
                outgoingMessageDto = await _chatService.AddMessageToContact(user, incMessageDto);
            }
            catch
            {
                return BadRequest();
            }

            await _hubContext.Clients.User(outgoingMessageDto.UserToSendId).SendAsync("sendNewMessage", JsonConvert.SerializeObject(outgoingMessageDto));

            return Ok(outgoingMessageDto);
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ReadMessage([FromBody] ContactIdDTO contactIdDto)
        {
            try
            {
                await _chatService.UpdateContactIsReaded(_userManager.GetUserId(User), contactIdDto.ContactID);
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
