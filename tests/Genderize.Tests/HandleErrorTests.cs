using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Genderize.Exceptions;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace Genderize.Tests
{
    [TestFixture]
    public class HandleErrorTests
    {
        [Test]
        public void CanHandleBadRequests()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.genderize.io/")
                .WithQueryString("name", "test")
                .Respond(HttpStatusCode.BadRequest, "application/json", "{\"error\":\"Error Message is Here\"}");
            var httpClient = mockHttp.ToHttpClient();

            var client = new GenderizeClient(httpClient);

           var exception = Assert.CatchAsync<BadRequestException>(async () =>
            {
                var result = await client.GetNameGender("test")
                    .ConfigureAwait(false);
            });

            Assert.AreEqual("Error Message is Here", exception.Message);
        }

        [Test]
        public void CanHandleTooManyRequests()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.genderize.io/")
                .WithQueryString("name", "test")
                .Respond((HttpStatusCode)429, "application/json", "{\"error\":\"Error Message is Here\"}");
            var httpClient = mockHttp.ToHttpClient();

            var client = new GenderizeClient(httpClient);

            var exception = Assert.CatchAsync<TooManyRequestsException>(async () =>
            {
                var result = await client.GetNameGender("test")
                    .ConfigureAwait(false);
            });

            Assert.AreEqual("Error Message is Here", exception.Message);
        }

        [Test]
        public void CanHandleInternalServerErrors()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.genderize.io/")
                .WithQueryString("name", "test")
                .Respond(HttpStatusCode.InternalServerError, "application/json", "{\"error\":\"Error Message is Here\"}");
            var httpClient = mockHttp.ToHttpClient();

            var client = new GenderizeClient(httpClient);

            var exception = Assert.CatchAsync<InternalServerErrorException>(async () =>
            {
                var result = await client.GetNameGender("test")
                    .ConfigureAwait(false);
            });

            Assert.AreEqual("Error Message is Here", exception.Message);
        }
    }
}
