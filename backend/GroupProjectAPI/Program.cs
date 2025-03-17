var builder = WebApplication.CreateBuilder(args);

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
