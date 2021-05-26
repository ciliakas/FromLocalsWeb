using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Services.EF;
using FromLocalsToLocals.Web.Controllers;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FromLocalsToLocals.xunit.SwipeCardTests
{
    public class SwipeCardControllerTests
    {
        private readonly AppDbContext _context;
        private readonly Mock<IVendorService> _vendorServiceMock;

        private readonly SwipeCardController _controller;

        public SwipeCardControllerTests()
        {
            _vendorServiceMock = new Mock<IVendorService>();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AppDatabase")
                .Options;

            try
            {
                var context = new AppDbContext(options);
                context.Vendors.Add(new Vendor { ID = 1, Latitude = 1, Longitude = 1, UserID = "Shop1" });
                context.Vendors.Add(new Vendor { ID = 2, Latitude = 1, Longitude = 1, UserID = "Shop2" });
                context.Vendors.Add(new Vendor { ID = 3, Latitude = 1, Longitude = 2, UserID = "Shop3" });
                context.Users.Add(new AppUser { Id = "1", UserName = "User1" });
                context.Users.Add(new AppUser { Id = "2", UserName = "User2" });
                context.Users.Add(new AppUser { Id = "3", UserName = "User3" });
                context.Reviews.Add(new Review { VendorID = 1, Stars = 4 });
                context.Reviews.Add(new Review { VendorID = 1, Stars = 4 });
                context.Reviews.Add(new Review { VendorID = 1, Stars = 3 });
                context.Reviews.Add(new Review { VendorID = 1, Stars = 3 });
                context.SaveChanges();
            }
            catch (Exception)
            {
                // ignored
            }

            _controller = new SwipeCardController(new AppDbContext(options), _vendorServiceMock.Object);

        }

        [Theory]
        [InlineData(1, 1, 1, 1, 0)]
        [InlineData(1, 1, 2, 1, 111.27396917709969)]
        public void GetDistance_ShouldCalculateCorrectDistance(double longitude, double latitude, double otherLongitude, double otherLatitude, double result)
        {
            var distance = _controller.GetDistance(longitude, latitude, otherLongitude, otherLatitude);
            distance.Should().Be(result);
        }

        [Theory]
        [InlineData("1", "User1")]
        [InlineData("2", "User2")]
        public void GetUserNameById_ShouldGetCorrectUserName(string name, string result)
        {
            var userName = _controller.GetUserNameById(name);
            userName.Should().Be(result);
        }

        [Theory]
        [InlineData(1, 3.5)]
        public void GetAverageScore_ShouldGetCorrectReviewScore(int id, double result)
        {
            var score = _controller.GetAverageScore(id);
            score.Should().Be(result);
        }

        [Theory]
        [InlineData(2, 0)]
        public void GetAverageScore_ShouldGetCorrectReviewScore_WhenVendorHasNoReviews(int id, double result)
        {
            var score = _controller.GetAverageScore(id);
            score.Should().Be(result);
        }
        [Theory]
        [InlineData(4, 0)]
        public void GetAverageScore_ShouldGetCorrectReviewScore_WhenVendorDoesNotExist(int id, double result)
        {
            var score = _controller.GetAverageScore(id);
            score.Should().Be(result);
        }


        //public double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        //public string GetUserNameById(string UserID)
        //public double GetAverageScore(int ID)
    }
}
