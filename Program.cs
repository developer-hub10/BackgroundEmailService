using BackgroundEmailService.Data;
using BackgroundEmailService.Services;
using BackgroundEmailService.IMappers;
using BackgroundEmailService.Mappers;
using BackgroundEmailService.Repository;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Authorization;


var builder = WebApplication.CreateBuilder(args);




// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options =>
   options.Filters.Add(new AuthorizeFilter())
);

// Dependency Injection
builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<IApplicantMapper, ApplicantMapper>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options => 
           options.TokenValidationParameters = new TokenValidationParameters
           {
             ValidateIssuer = true,
             ValidIssuer = builder.Configuration["AppSettings:Issuer"],
             ValidateAudience = true,
             ValidAudience = builder.Configuration["AppSettings:Audience"],
             ValidateLifetime = true,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
             ValidateIssuerSigningKey = true,
           }
       );


// Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnection"))
));


var app = builder.Build();


app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();



app.Run();

