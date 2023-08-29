using AdoptiverseAPI.DataAccess;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

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

        }
    }
}