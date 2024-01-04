using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Driver;
using SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allowanyorigin", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
}).AddHubOptions<SignalRHub>(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromMinutes(60);
});

builder.Services.AddSingleton<IMongoClient>(new MongoClient("mongodb+srv://PollsAPI:GkpwsTu9GSLGgVW@cluster0.1hrsxov.mongodb.net/?retryWrites=true&w=majority"));

var app = builder.Build();

app.UseCors("Allowanyorigin");

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
var mongoClient = app.Services.GetRequiredService<IMongoClient>();

lifetime.ApplicationStarted.Register(() =>
{
    var collection = mongoClient.GetDatabase("Speedruns").GetCollection<BsonDocument>("polls");

    using (var cursor = collection.Watch())
    {
        foreach (var change in cursor.ToEnumerable())
        {
            if (change.OperationType == ChangeStreamOperationType.Insert)
            {
                var hubContext = app.Services.GetRequiredService<IHubContext<SignalRHub>>();
                hubContext.Clients.All.SendAsync("ReceiveMessage", "poll created"); // Broadcast the change to connected clients
            }
        }
    }
});

app.MapHub<SignalRHub>("/Signals");

app.Run();
