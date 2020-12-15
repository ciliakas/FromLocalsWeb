using FromLocalsToLocals.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Web.Models.Services
{
    public interface IReviewsService
    {
        Task CreateAsync(Review review);
        Task AddReplyAsync(int id, int index , string text, string sender);
        Task<Review> GetReviewAsync(int id, int index);
        Task<List<Review>> GetReviewsAsync(int id);
    }
}
