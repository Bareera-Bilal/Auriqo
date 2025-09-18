using Microsoft.EntityFrameworkCore;
using Auriqo_Web_Api_Backend.Interfaces;
using Auriqo_Web_Api_Backend.Models.JunctionModels;
using Auriqo_Web_Api_Backend.Services;
using Auriqo_Web_Api_Backend;
using Microsoft.AspNetCore.Authentication.Cookies;




var builder = WebApplication.CreateBuilder(args);

// ----------------- Services Registration -----------------

builder.Services.AddControllers();




// Database context
builder.Services.AddDbContext<SqlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("local")));




// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";              // Redirect users who aren’t logged in
        options.AccessDeniedPath = "/Error/AccessDenied"; // Redirect for unauthorized access
    });





// Session handling
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});





// CORS policy (allowing frontend to access backend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:7687")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});





// Register Email settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));





// Cloudinary & JWT config
var cloudinaryUri = builder.Configuration["Cloudinary:URI"] ?? throw new InvalidOperationException("Cloudinary url not set !");
var SecretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException(" Secret Key not set !");






// Dependency Injection
builder.Services.AddSingleton<ICloudinaryService>(_ => new CloudinaryService(cloudinaryUri));
builder.Services.AddSingleton<ITokenService>(_ => new TokenService(SecretKey));
builder.Services.AddSingleton<IMailService, EmailService>();




// ----------------- Build App -----------------
var app = builder.Build();





// ----------------- Middleware Pipeline -----------------

// Development Tools
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}



app.UseHttpsRedirection();

// Use authentication & session
app.UseAuthentication();
app.UseSession();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
