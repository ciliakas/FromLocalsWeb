using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.DTO;
using FromLocalsToLocals.Contracts.Entities;

namespace FromLocalsToLocals.Services.EF
{
    public interface IChatService
    {
        Task CreateContact(Contact contact);
        Task<OutGoingMessageDTO> AddMessageToContact(AppUser user, IncomingMessageDTO message);
        Task UpdateContactIsReaded(string userId, int contactId);
    }
}