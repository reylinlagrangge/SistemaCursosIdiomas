using Idiomas.Application.Contract;
using Idiomas.Application.Services;
using Idiomas.Infrastructure.Context;
using Idiomas.Infrastructure.Interfaces;
using Idiomas.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LanguageSchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LanguageSchoolDb")));

// Repositorios
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Servicios (capa Application)
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
