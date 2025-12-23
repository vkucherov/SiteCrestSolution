using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SiteCrestBackend;
using SiteCrestBackend.Models;
using System;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace TestsProject
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UnitTest1(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");

                builder.ConfigureServices(services =>
                {
                    // Remove existing DbContext registrations
                    var descriptors = services
                        .Where(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>))
                        .ToList();

                    foreach (var d in descriptors)
                        services.Remove(d);

                    // IMPORTANT: fixed DB name
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });
                });
            });

            // ✅ Seed AFTER host is built
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Pathways.Add(new Pathway { Id = 1, Name = "Pathway 1", SortValue = 1 });
            db.Chemicals.Add(new Chemical { Id = 1, Name = "Chemical 1" });

            db.SaveChanges();

        }

        [Fact]
        public void Test1()
        {
            // test of test
            Assert.True(true);
        }

        [Fact]
        public async Task Get_Chemicals_ReturnsSeededChemical()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/manage/chemicals");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            Assert.True(root.ValueKind == JsonValueKind.Array);

            bool found = false;
            foreach (var el in root.EnumerateArray())
            {
                if (el.TryGetProperty("id", out var idProp) &&
                    idProp.GetInt32() == 1 &&
                    el.TryGetProperty("name", out var nameProp) &&
                    nameProp.GetString() == "Chemical 1")
                {
                    found = true;
                }
            }

            Assert.True(found, "Seeded chemical not found in GET /manage/chemicals response.");
        }

        [Fact]
        public async Task Post_Values_CreatesOrUpdatesValue()
        {
            var client = _factory.CreateClient();

            var payload = new
            {
                ChemicalId = 1,
                Values = new[]
                {
                    new { PathwayId = 1, Value = 2.5 }
                }
            };

            var postResponse = await client.PostAsJsonAsync("/manage/values", payload);
            postResponse.EnsureSuccessStatusCode();

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var cp = await db.ChemicalPathways.FirstOrDefaultAsync(x => x.ChemicalId == 1 && x.PathwayId == 1);
            Assert.NotNull(cp);
            Assert.Equal(2.5, cp.Value);
        }
    }
}
