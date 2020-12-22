using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Services.EF
{
    public class ReviewsService : IReviewsService
    {
        private readonly AppDbContext _context;

        public ReviewsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddReplyAsync(int id, int index ,string text, string sender)
        {
            try
            {
                var review = await GetReviewAsync(id, index);

                review.Reply = text;
                review.ReplySender = sender;
                review.ReplyDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

                _context.Update(review);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw new DbUpdateException("Unable to save a reply");
            }
        }

        public async Task CreateAsync(Review review)
        {
            try
            {
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Unable to save review");
            }
        }

        public Task<Review> GetReviewAsync(int id, int index)
        {
            return _context.Reviews.FirstOrDefaultAsync(x => (x.VendorID == id) && (x.CommentID == index));
        }

        public async Task<List<Review>> GetReviewsAsync(int id)
        {
            return await _context.Reviews.Where(x => x.VendorID == id).ToListAsync();
        }
    }
}
