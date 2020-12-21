using FromLocalsToLocals.Contracts.DTO;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Services.EF
{
    public class ChatService : IChatService
    {
        private readonly AppDbContext _context;

        public ChatService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OutGoingMessageDTO> AddMessageToContact(AppUser user, IncomingMessageDTO message)
        {
            var outgoingMessage = new OutGoingMessageDTO() { Message = message.Message, ContactID = message.ContactId, IsUserTab = message.IsUserTab };
            Contact contact = null;

            if (message.IsUserTab)
            {

                contact = user.Contacts.FirstOrDefault(x => x.ID == message.ContactId);
                if (contact == null)
                {
                    throw new Exception("This contact doesn't 'belong' to this user");
                }
                outgoingMessage.UserToSendId = contact.Vendor.UserID;
                outgoingMessage.Image = user.Image;

            }
            else
            {
                user.Vendors.ToList().ForEach(x =>
                {
                    var c = x.Contacts.FirstOrDefault(y => y.ID == message.ContactId);
                    if (c != null)
                    {
                        contact = c;
                        outgoingMessage.UserToSendId = contact.UserID;
                        outgoingMessage.Image = x.Image;
                        return;
                    }
                });
            }

            try
            {
                contact.Messages.Add(new Message { Text = message.Message , Contact = contact, IsUserSender = message.IsUserTab });
                contact.ReceiverRead = !message.IsUserTab;
                contact.UserRead = message.IsUserTab;

                _context.Update(contact);
                await _context.SaveChangesAsync();

                outgoingMessage.VendorTitle = contact.Vendor.Title;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return outgoingMessage;
        }

        public async Task CreateContact(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateContactIsReaded(string userId, int contactId)
        {
            try
            {
                var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.ID == contactId);
                contact.ReceiverRead = true;
                contact.UserRead = true;
                _context.Contacts.Update(contact);
                await _context.SaveChangesAsync();

            }
            catch
            {
                return;
            }
        }
    }
}
