using Planner.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Planner.Application.Interfaces.Services;
using Planner.Application.Interfaces.Repositories;
using Planner.Application.Interfaces.Utilities;
using Planner.Infrastructure.Repositories;
using Planner.Application.Utilities;
using Planner.Api.Middlewares;
using Planner.Api.Mappers.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITranslationUtility, TranslationUtility>();

builder.Services.AddScoped<IGoalRepository, GoalRepository>();

builder.Services.AddScoped<IGoalService, GoalService>();

builder.Services.AddDbContext<PlannerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

UtilityMappings.ServiceProvider = app.Services;

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();