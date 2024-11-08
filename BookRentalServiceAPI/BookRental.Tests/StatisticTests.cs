using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Tests
{
    public class StatisticTests : IntegrationTestBase
    {
        public StatisticTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task GetMostPopularBook_ShouldReturnBook()
        {
            var response = await Client.GetAsync("/api/statistics/most-popular-book");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetLeastPopularBook_ShouldReturnBook()
        {
            var response = await Client.GetAsync("/api/statistics/least-popular-book");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetMostOverdueBook_ShouldReturnBook()
        {
            var response = await Client.GetAsync("/api/statistics/most-overdue-book");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
