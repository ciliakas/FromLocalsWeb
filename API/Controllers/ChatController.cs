using FromLocalsToLocals.Contracts.DTO;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Services.EF;
using FromLocalsToLocals.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FromLocalsToLocals.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = MvcClientConstants.AuthSchemes)]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly UserManager<AppUser> _userManager;

        public ChatController(IChatService chatService, UserManager<AppUser> userManager)
        {
            _chatService = chatService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateMessage([FromBody] IncomingMessageDTO incMessageDto)
        {
            OutGoingMessageDTO outgoingMessageDto = new OutGoingMessageDTO();
            try
            {
                var user = await _userManager.GetUserAsync(User);
                outgoingMessageDto = await _chatService.AddMessageToContact(user, incMessageDto);
            }
            catch
            {
                return BadRequest();
            }
            return Ok(outgoingMessageDto);
        }

        
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ReadMessage([FromBody]ContactIdDTO contactIdDto)
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
