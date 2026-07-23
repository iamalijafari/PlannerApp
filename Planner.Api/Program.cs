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
            .AllowAnyMethod();

        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin();
        }
        else
        {
            policy.WithOrigins("http://localhost:3000");
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITranslationUtility, TranslationUtility>();

builder.Services.AddScoped<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<IYearlyPlanRepository, YearlyPlanRepository>();
builder.Services.AddScoped<IMonthlyPlanRepository, MonthlyPlanRepository>();
builder.Services.AddScoped<IWeeklyPlanRepository, WeeklyPlanRepository>();
builder.Services.AddScoped<IDailyPlanRepository, DailyPlanRepository>();

builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<IYearlyPlanService, YearlyPlanService>();
builder.Services.AddScoped<IMonthlyPlanService, MonthlyPlanService>();
builder.Services.AddScoped<IWeeklyPlanService, WeeklyPlanService>();
builder.Services.AddScoped<IDailyPlanService, DailyPlanService>();
builder.Services.AddScoped<ITranslationService, TranslationService>();

builder.Services.AddDbContext<PlannerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PlannerDbContext>();
    db.Database.Migrate();
}

app.UseCors("AllowUI");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.MapGet("/health", () => Results.Ok("Healthy"));

app.Run();