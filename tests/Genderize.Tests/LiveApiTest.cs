using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Genderize.Tests
{
    [TestFixture]
    public class LiveApiTest
    {
        [Test]
        public async Task MaleName()
        {
            var client = new GenderizeClient();
            var result = await client.GetNameGender("João");
            Assert.AreEqual(Gender.Male, result.Gender);
        }

        [Test]
        public async Task FemaleName()
        {
            var client = new GenderizeClient();
            var result = await client.GetNameGender("Maria");
            Assert.AreEqual(Gender.Female, result.Gender);
        }
    }
}
