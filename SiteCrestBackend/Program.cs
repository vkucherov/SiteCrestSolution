using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SiteCrestBackend.Models;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("AppTestsDb"));
}
else
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        policy
            .WithOrigins("https://localhost:7095")
            .WithOrigins("https://sitecrestfront-hmh4cbffc8brdkcd.canadacentral-01.azurewebsites.net")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseCors("BlazorClient");

// Ensure DB is created at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// get all chemicals (project to DTO to avoid JSON cycles)
app.MapGet("/manage/chemicals", async (AppDbContext db) =>
    await db.Chemicals
            .OrderBy(c => c.Name)
            .Select(c => new ChemicalDto
            {
                Id = c.Id,
                Name = c.Name ?? string.Empty,
                Values = c.Values.Select(v => new ChemicalPathwayDto
                {
                    PathwayId = v.PathwayId,
                    PathwayName = v.Pathway.DisplayName,
                    Value = v.Value
                }).ToList()
            })
            .ToListAsync());

// get a specific chemical by its ID
app.MapGet("/manage/chemicals/{id}", async (int id, AppDbContext db) =>
{
    var chemical = await db.Chemicals
        .Where(c => c.Id == id)
        .Select(c => new EditChemicalValuesDto
        {
            ChemicalId = c.Id,
            ChemicalName = c.Name ?? string.Empty,
            Values = c.Values.Select(v => new ChemicalPathwayDto
            {
                PathwayId = v.PathwayId,
                PathwayName = v.Pathway.DisplayName ?? string.Empty,
                Value = v.Value
            }).ToList()
        })
        .FirstOrDefaultAsync();

    return chemical != null ? Results.Ok(chemical) : Results.NotFound();
});

app.MapPost("/manage/chemicals", async (ChemicalDto dto, AppDbContext db) =>
{
    if (dto == null || dto.Name.IsNullOrEmpty()) return Results.BadRequest("Missing required fields."); // 400

    if (db.Chemicals.Any(c => c.Name == dto.Name)) return Results.Conflict("Chemical already exists."); // 409

    var chemical = new Chemical
    {
        Name = dto.Name
    };

    db.Chemicals.Add(chemical);
    await db.SaveChangesAsync();

    return Results.Created($"/manage/chemicals/{dto.Id}", dto);
});

// update a chemical
app.MapPut("/manage/chemicals/{id}", async (int id, EditChemicalValuesDto inputChemical, AppDbContext db) =>
{
    var chemical = await db.Chemicals.FindAsync(id);

    if (chemical is null) return Results.NotFound();

    if (db.Chemicals.Any(c => c.Name == inputChemical.ChemicalName)) return Results.Conflict("Chemical name already exists."); // 409

    chemical.Name = inputChemical.ChemicalName;

    await db.SaveChangesAsync();

    return Results.NoContent();
});


// delete a chemical
app.MapDelete("/manage/chemicals/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Chemicals.FindAsync(id) is Chemical chemical)
    {
        db.Chemicals.Remove(chemical);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});


// ==== Pathways

// get all pathways
app.MapGet("/manage/pathways", async (AppDbContext db) =>
    await db.Pathways
        .OrderBy(p => p.SortValue)
        .Select(p => new PathwayDto
        {
            Id = p.Id,
            Name = p.Name,
            DisplayName = p.DisplayName,
            SortValue = p.SortValue
        })
        .ToListAsync());

// get a specific pathway by its ID
app.MapGet("/manage/pathways/{id}", async (int id, AppDbContext db) =>
    await db.Pathways
        .Where(p => p.Id == id)
        .Select(p => new PathwayDto
        {
            Id = p.Id,
            Name = p.Name,
            DisplayName = p.DisplayName,
            SortValue = p.SortValue
           
        })
        .FirstOrDefaultAsync());


// === Set value

app.MapPost("/manage/values", async (EditChemicalValuesDto dto, AppDbContext db) =>
{
    var chemical = await db.Chemicals
        .Include(c => c.Values)
        .FirstOrDefaultAsync(c => c.Id == dto.ChemicalId);

    if (chemical == null)
        return Results.NotFound();

    if (dto.Values.Any(v => v.Value < 0))
    {
        return Results.BadRequest("Values cannot be negative."); // 400
    }

    foreach (var val in dto.Values)
    {
        var existing = chemical.Values.FirstOrDefault(v => v.PathwayId == val.PathwayId);
        if (existing != null)
        {
            existing.Value = val.Value; // update existing
        }
        else
        {
            chemical.Values.Add(new ChemicalPathway
            {
                PathwayId = val.PathwayId,
                Value = val.Value
            });
        }
    }

    await db.SaveChangesAsync();
    return Results.Ok();
});


app.Run();


