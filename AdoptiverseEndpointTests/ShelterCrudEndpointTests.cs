using AdoptiverseAPI.DataAccess;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using AdoptiverseAPI.Models;
using static System.Reflection.Metadata.BlobBuilder;
using System.Text;

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
                NamingStrategy = new CamelCaseNamingStrategy()
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

        [Fact]
        public async void ReturnShelter_ReturnsSingleShelter()
        {
            HttpClient client = _factory.CreateClient();
            AdoptiverseApiContext context = GetDbContext();

            Shelter shelter1 = new Shelter { Name = "Smart Friend League", Rank = 3, City = "denver" };
            Shelter shelter2 = new Shelter { Name = "BigBrain Friend League", Rank = 9, City = "Aurora" };

            List<Shelter> shelters = new() { shelter1, shelter2 };
            context.AddRange(shelters);
            context.SaveChanges();

            //act
            HttpResponseMessage response = await client.GetAsync($"/api/shelters/{shelter1.Id}");
            string content = await response.Content.ReadAsStringAsync();

            string expected = ObjectToJson(shelter1);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.DoesNotContain(ObjectToJson(shelter2), content);
            Assert.Equal(expected, content);

        }

        [Fact]
        public async void UpdateShelter_UpdatesExsistingshelter()
        {
            HttpClient client = _factory.CreateClient();
            AdoptiverseApiContext context = GetDbContext();

            Shelter shelter1 = new Shelter { Name = "Smart Friend League", Rank = 3, City = "denver" };
            context.Add(shelter1);
            context.SaveChanges();

            var jsonString = "{\"CreatedAt\": \"2023-08-29T12:00:00.000Z\", \"UpdatedAt\": \"2023-08-29T12:05:00.000Z\", \"HasFosterProgram\": true, \"Rank\": 1, \"City\": \"San Francisco\", \"Name\": \"Happy Paws\"}";
            var requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/shelters/1", requestContent);

            context.ChangeTracker.Clear();

            Assert.Equal(204, (int)response.StatusCode);
            Assert.Equal("Happy Paws", context.Shelters.Find(1).Name);


        }

        [Fact]
        public async void DeleteShelter_DeletesExsistingShelter()
        {
            HttpClient client = _factory.CreateClient();
            AdoptiverseApiContext context = GetDbContext();

            Shelter shelter1 = new Shelter { Name = "Smart Friend League", Rank = 3, City = "denver" };
            context.Add(shelter1);
            context.SaveChanges();


            var response = await client.DeleteAsync("/api/shelters/1");
            var content = await response.Content.ReadAsStringAsync();

            //Assert.Equal(204, (int)response.StatusCode);
            Assert.DoesNotContain("denver", content);
        }
    }
}