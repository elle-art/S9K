using Google.Api;
using Google.Cloud.Firestore;
using Backend.Models;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://10.0.0.202:5000");

// Set Firebase credentials (only once at startup)
string keyPath = Path.Combine(Directory.GetCurrentDirectory(), "firebase", "firebase-key.json");
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", keyPath);

// FirestoreDb can be reused or dependency injected later
FirestoreDb db = FirestoreDb.Create("the-scheduler-9000");

// Add services to the container.
builder.Services.AddControllers();  // Registers API controllers
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAll");  // Apply the CORS policy

// app.UseHttpsRedirection();

app.UseAuthorization();  // Important if you plan to add authentication

app.MapControllers();  // Maps all controllers automatically

app.Use(async (context, next) =>
{
    Console.WriteLine($"Incoming request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
});

app.Run();
