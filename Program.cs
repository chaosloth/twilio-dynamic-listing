using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Twilio.Example.Models;
using Twilio.Example.Data;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure Database
builder.Services.AddDbContext<CallTrackingContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CallTrackingContext") ?? throw new InvalidOperationException("Connection string 'CallTrackingContext' not found.")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Twilio Example for Dynamic Numbers in Listings",
        Description = "An example app that uses Twilio phone numbers for eCommerce listings. Build with ASP.NET Core Web API and Twilio Client to create, manage and delete phone number association with listings.",
        TermsOfService = new Uri("https://twilio.com/"),
        Contact = new OpenApiContact
        {
            Name = "Christopher Connolly",
            Url = new Uri("http://github.com/chaosloth")
        }
    });
});

builder.Services.AddSwaggerGen(c => { c.EnableAnnotations(); });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //Add swagger
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();

app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dojo App"));

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();

app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
{
    var sb = new System.Text.StringBuilder();
    var endpoints = endpointSources.SelectMany(es => es.Endpoints);
    foreach (var endpoint in endpoints)
    {
        if (endpoint is RouteEndpoint routeEndpoint)
        {
            Console.WriteLine($"Display name {endpoint.DisplayName}");
            Console.WriteLine($" - Raw text {routeEndpoint.RoutePattern.RawText}");
        }
    }
});

app.Run();