using System;
using Hangfire;
using Hangfire.Storage.MySql;
using BackgroundEmailService.Data;
using BackgroundEmailService.Services;
using BackgroundEmailService.IMappers;
using BackgroundEmailService.Mappers;
using BackgroundEmailService.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Add configuration and services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options =>
    options.Filters.Add(new AuthorizeFilter())
);

// 2. Dependency Injection
builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<IApplicantMapper, ApplicantMapper>();
builder.Services.AddScoped<MyBackgroundService>();

// 3. Entity Framework Core (MySQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnection"))
    )
);

// 4. Authentication (JWT Bearer)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)
            ),
            ValidateIssuerSigningKey = true,
        };
    });

// 5. Hangfire Configuration (MySQL Storage)
builder.Services.AddHangfire(config =>
    config.UseStorage(new MySqlStorage(
        builder.Configuration.GetConnectionString("HangfireConnection"),
        new MySqlStorageOptions()
    ))
);
builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = Environment.ProcessorCount * 2;; 
});

var app = builder.Build();



// 6. Middleware pipeline
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// 7. Hangfire Dashboard & Recurring Job
app.UseHangfireDashboard("/hangfire");
RecurringJob.AddOrUpdate<MyBackgroundService>(
    "send-email-job",
    job => job.ExecuteAsync(),
    Cron.MinuteInterval(2)
);

app.Run();
