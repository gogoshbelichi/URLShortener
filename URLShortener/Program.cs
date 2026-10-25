using URLShortener.Infrastructure;
using URLShortener.Options;
using URLShortener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IUrlStorage, UrlStorage>();
builder.Services.AddScoped<IBase62Converter, Base62Converter>();
builder.Services.AddScoped<IUrlShortenerService, UrlShortenerService>();
builder.Services.AddOptions<UrlShortenerOptions>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();