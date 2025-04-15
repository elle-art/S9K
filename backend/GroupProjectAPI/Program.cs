using Google.Cloud.Firestore;

var builder = WebApplication.CreateBuilder(args);

// Set Firebase credentials (only once at startup)
string keyPath = Path.Combine(Directory.GetCurrentDirectory(), "firebase", "firebase-key.json");
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", keyPath);

// FirestoreDb can be reused or dependency injected later
FirestoreDb db = FirestoreDb.Create("your-project-id");

// Add services to the container.
builder.Services.AddControllers();  // ✅ Registers API controllers
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAll");  // ✅ Apply the CORS policy

// app.UseHttpsRedirection();

app.UseAuthorization();  // ✅ Important if you plan to add authentication

app.MapControllers();  // ✅ Maps all controllers automatically

app.Run();
