using Planner.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Planner.Application.Interfaces.Services;
using Planner.Application.Interfaces.Repositories;
using Planner.Application.Interfaces.Utilities;
using Planner.Infrastructure.Repositories;
using Planner.Application.Utilities;
using Planner.Api.Middlewares;
using Planner.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000");
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITranslationUtility, TranslationUtility>();

builder.Services.AddScoped<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<IYearlyGoalRepository, YearlyGoalRepository>();

builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<IYearlyGoalService, YearlyGoalService>();
builder.Services.AddScoped<ITranslationService, TranslationService>();

builder.Services.AddDbContext<PlannerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseCors("AllowUI");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();