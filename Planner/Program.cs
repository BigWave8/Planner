using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Planner.Repositories;
using Planner.Data;
using Planner.Services.Interfaces;
using Planner.Services;
using Planner.Repositories.Interfaces;
using FluentValidation;
using Planner.DTOs;
using Planner.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PlannerDBContext>(options => options.UseInMemoryDatabase(databaseName: "MockPlannerDb"));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IToDoListService, ToDoListService>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IToDoListRepository, ToDoListRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddScoped<IValidator<TaskDTO>, TaskValidator>();
builder.Services.AddScoped<IValidator<ToDoListDTO>, ToDoListValidator>();
builder.Services.AddScoped<IValidator<UserDTO>, UserValidator>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
