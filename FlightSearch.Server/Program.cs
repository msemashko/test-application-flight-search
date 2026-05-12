using FlightSearch.Application.Validators;
using FlightSearch.Infrastructure;
using FlightSearch.Infrastructure.Data;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddValidatorsFromAssemblyContaining<FlightSearchRequestValidator>();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Flight Search API"));
}

app.UseCors();

app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
