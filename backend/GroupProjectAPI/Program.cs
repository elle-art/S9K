using Google.Api;
using Google.Cloud.Firestore;
using Backend.Models;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://172.16.2.249:5000");

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

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var app = builder.Build();

app.MapGet("/test-dummy", async () =>
{
    // 1. Create a Dummy instance
    Dummy originalDummy = new Dummy("Carl", 42);
    Console.WriteLine("Dummy object sent with name " + originalDummy.Name + " and number " + originalDummy.Number);

    // 2. Save to Firestore
    DocumentReference docRef = db.Collection("dummyTests").Document("test1");
    await docRef.SetAsync(originalDummy);

    // 3. Retrieve it back
    DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

    if (snapshot.Exists)
    {
        Dummy retrievedDummy = snapshot.ConvertTo<Dummy>();
        Console.WriteLine("Dummy object retrieved with name " + retrievedDummy.Name + " and number " + retrievedDummy.Number);
        return Results.Json(new {
            success = true,
            name = retrievedDummy.Name,
            number = retrievedDummy.Number
        });

        
    }
    else
    {
        return Results.Json(new { success = false, error = "Document not found" });
    }
});

app.UseCors("AllowAll");  //Apply the CORS policy

// app.UseHttpsRedirection();

app.UseAuthorization();  // Important if you plan to add authentication

app.MapControllers();  // Maps all controllers automatically

app.Use(async (context, next) =>
{
    Console.WriteLine($"Incoming request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
});

app.Run();
