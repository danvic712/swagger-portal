using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddIngosSwagger(options =>
{
    options.OpenApiInfo = new OpenApiInfo()
    {
        Title = "ASP.NET Core 6.0 Web API Project",
        Contact = new OpenApiContact()
        {
            Email = "fake@email.com"
        }
    };
});;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseIngosSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();