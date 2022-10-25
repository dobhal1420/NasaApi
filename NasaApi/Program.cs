using NasaApi.Client;
using NasaApi.Policies.RequestService.Policies;
using NasaApi.Service;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Registering polly retry Policy
builder.Services.AddSingleton(new ClientPolicy());

builder.Services.AddScoped<INasaImageRetriever, NasaImageRetriever>();
builder.Services.AddHttpClient<INasaClient,NasaClient>();
builder.Services.AddMemoryCache();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
