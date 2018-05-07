using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace Genderize.Tests
{
    [TestFixture]
    public class ClientTests
    {
        [Test]
        public async Task CanReadMaleResponse()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.genderize.io/")
                .WithQueryString("name", "João")
                .Respond("application/json", "{\"name\":\"Test\",\"gender\":\"male\",\"probability\":1,\"count\":156}");
            var httpClient = mockHttp.ToHttpClient();

            var client = new GenderizeClient(httpClient);
            var result = await client.GetNameGender("João")
                .ConfigureAwait(false);

            Assert.AreEqual(Gender.Male, result.Gender);
            Assert.AreEqual(1, result.Probability);
            Assert.AreEqual(156, result.Count);
            Assert.AreEqual("Test", result.Name);
        }

        [Test]
        public async Task CanReadFemaleResponse()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.genderize.io/")
                .WithQueryString("name", "Maria")
                .Respond("application/json", "{\"name\":\"Test\",\"gender\":\"female\",\"probability\":0.99,\"count\":8402}");
            var httpClient = mockHttp.ToHttpClient();

            var client = new GenderizeClient(httpClient);
            var result = await client.GetNameGender("Maria")
                .ConfigureAwait(false);

            Assert.AreEqual(Gender.Female, result.Gender);
            Assert.AreEqual(0.99f, result.Probability);
            Assert.AreEqual(8402, result.Count);
            Assert.AreEqual("Test", result.Name);
        }

        [Test]
        public async Task CanSendCountry()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.genderize.io/")
                .WithQueryString(new Dictionary<string, string>
                {
                    {"name", "João"},
                    {"country_id", "pt"}
                })
                .Respond("application/json", "{\"name\":\"Test\",\"gender\":\"male\",\"probability\":1,\"count\":156}");
            var httpClient = mockHttp.ToHttpClient();

            var client = new GenderizeClient(httpClient);
            var result = await client.GetNameGender("João", country: "pt")
                .ConfigureAwait(false);

            Assert.AreEqual(Gender.Male, result.Gender);
            Assert.AreEqual(1, result.Probability);
            Assert.AreEqual(156, result.Count);
            Assert.AreEqual("Test", result.Name);
        }

        [Test]
        public async Task CanSendLanguage()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.genderize.io/")
                .WithQueryString(new Dictionary<string, string>
                {
                    {"name", "João"},
                    {"language_id", "pt"}
                })
                .Respond("application/json", "{\"name\":\"Test\",\"gender\":\"male\",\"probability\":1,\"count\":156}");
            var httpClient = mockHttp.ToHttpClient();

            var client = new GenderizeClient(httpClient);
            var result = await client.GetNameGender("João", language: "pt")
                .ConfigureAwait(false);

            Assert.AreEqual(Gender.Male, result.Gender);
            Assert.AreEqual(1, result.Probability);
            Assert.AreEqual(156, result.Count);
            Assert.AreEqual("Test", result.Name);
        }

        [Test]
        public async Task CanSendLanguageAndCountry()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.genderize.io/")
                .WithQueryString(new Dictionary<string, string>
                {
                    {"name", "João"},
                    {"language_id", "pt"},
                    {"country_id", "pt" }
                })
                .Respond("application/json", "{\"name\":\"Test\",\"gender\":\"male\",\"probability\":1,\"count\":156}");
            var httpClient = mockHttp.ToHttpClient();

            var client = new GenderizeClient(httpClient);
            var result = await client.GetNameGender("João", country: "pt", language: "pt")
                .ConfigureAwait(false);

            Assert.AreEqual(Gender.Male, result.Gender);
            Assert.AreEqual(1, result.Probability);
            Assert.AreEqual(156, result.Count);
            Assert.AreEqual("Test", result.Name);
        }

        [Test]
        public async Task CanHandleNullReturns()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.genderize.io/")
                .WithQueryString("name", "hippopotamus")
                .Respond("application/json", "{\"name\":\"hippopotamus\",\"gender\":null}");
            var httpClient = mockHttp.ToHttpClient();

            var client = new GenderizeClient(httpClient);
            var result = await client.GetNameGender("hippopotamus")
                .ConfigureAwait(false);

            Assert.IsNull(result.Gender);
            Assert.AreEqual("hippopotamus", result.Name);
        }
    }
}
