﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;

namespace BookRental.Tests
{
    public class AuthenticationTests : IntegrationTestBase
    {
        public AuthenticationTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task RegisterUser_ShouldReturnSuccess()
        {
            var response = await Client.PostAsJsonAsync("/api/auth/register", new
            {
                Username = "nihal.latif@test.com",
                Password = "12345678",
                Role = "User"
            });

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task LoginUser_ShouldReturnSuccess()
        {
            var response = await Client.PostAsJsonAsync("/api/auth/login", new
            {
                Username = "nihal.latif.yt@gmail.com",
                Password = "12345678",
                Role = "User"
            });

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
