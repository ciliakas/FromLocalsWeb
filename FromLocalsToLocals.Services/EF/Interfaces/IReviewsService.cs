using System.Collections.Generic;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;

namespace FromLocalsToLocals.Services.EF
{
    public interface IReviewsService
    {
        Task CreateAsync(Review review);
        Task AddReplyAsync(int id, int index, string text, string sender);
        Task<Review> GetReviewAsync(int id, int index);
        Task<List<Review>> GetReviewsAsync(int id);
    }
}