using AdoptiverseAPI.DataAccess;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using AdoptiverseAPI.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace AdoptiverseEndpointTests
{
    public class ShelterCrudEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ShelterCrudEndpointTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        private AdoptiverseApiContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AdoptiverseApiContext>();
            optionsBuilder.UseInMemoryDatabase("TestDatabase");

            var context = new AdoptiverseApiContext(optionsBuilder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }
        private string ObjectToJson(object obj)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            string json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            });

            return json;
        }

        [Fact]
        public async void GetShelters_ReturnsJSONListOfAllShelters()
        {
            HttpClient client = _factory.CreateClient();
            AdoptiverseApiContext context = GetDbContext();

            Shelter shelter1 = new Shelter { Name = "Smart Friend League", Rank = 3, City = "denver" };
            Shelter shelter2 = new Shelter { Name = "BigBrain Friend League", Rank = 9, City = "Aurora" };

            List<Shelter> shelters = new() { shelter1, shelter2 };
            context.AddRange(shelters);
            context.SaveChanges();

            //act
            HttpResponseMessage response = await client.GetAsync("/api/shelters");
            string content = await response.Content.ReadAsStringAsync();

            string expected = ObjectToJson(shelters);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(expected, content);

        }
    }
}