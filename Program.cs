
using Microsoft.EntityFrameworkCore;
using Auriqo_Web_Api_Backend.Interfaces;
using Auriqo_Web_Api_Backend.Services;
using Auriqo;
using Auriqo_Web_Api_Backend.Models.JunctionModels;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SqlDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("cloud")));

// cors policy for allowing frontend to send request on this server 

builder.Services.AddCors(Options => {Options.AddPolicy("AllowFrontend", policy => 
policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod().AllowCredentials());});

// register or configure the  options needed by EmailService having type of EmailSettings present in P0 classlibarary   
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

var cloudinaryUri = builder.Configuration["Cloudinary:URI"] ?? throw new InvalidOperationException("Cloudinary url not set !");
var SecretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException(" Secret Key not set !");



// not regular you have to use lambda expression 
builder.Services.AddSingleton<ICloudinaryService>(_ => new CloudinaryService(cloudinaryUri));
builder.Services.AddSingleton<ITokenService>(_ => new TokenService(SecretKey));


// regular as usual   // but complexity behind
builder.Services.AddSingleton<IMailService, EmailService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();


app.UseCors("AllowFrontend"); 

// app.UseAuthorization(); 

app.MapControllers();



app.Run();