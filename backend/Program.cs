using WebAPI.Settings;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using Microsoft.Extensions.FileProviders;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
    var mongoConnectionString =
    builder.Configuration["MongoDbSettings:ConnectionString"];

    var mongoDatabaseName =
    builder.Configuration["MongoDbSettings:DatabaseName"];

var mongoClient = new MongoClient(mongoConnectionString);

builder.Services.AddSingleton<IMongoClient>(mongoClient);

builder.Services.AddSingleton<IMongoDatabase>(
    mongoClient.GetDatabase(mongoDatabaseName));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/uploads"
});

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();