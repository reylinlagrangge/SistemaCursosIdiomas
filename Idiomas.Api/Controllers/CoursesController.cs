using Idiomas.Domain.Entities;
using Idiomas.Infrastructure.Exceptions;
using Idiomas.Infrastructure.Interfaces;
using Idiomas.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Idiomas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseRepository _repository;

    public CoursesController(ICourseRepository repository) => _repository = repository;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _repository.GetAllAsync();
        return Ok(courses.Select(ToModel));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var course = await _repository.GetByIdAsync(id);
        return course is null ? NotFound() : Ok(ToModel(course));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseModel model)
    {
        try
        {
            var created = await _repository.AddAsync(ToEntity(model));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToModel(created));
        }
        catch (CourseException ex) { return Conflict(ex.Message); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CourseModel model)
    {
        if (await _repository.GetByIdAsync(id) is null) return NotFound();
        model.Id = id;
        await _repository.UpdateAsync(ToEntity(model));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (await _repository.GetByIdAsync(id) is null) return NotFound();
        await _repository.DeleteAsync(id);
        return NoContent();
    }

    private static CourseModel ToModel(Course c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Language = c.Language,
        Description = c.Description,
        Price = c.Price,
        LevelId = c.LevelId,
        TeacherId = c.TeacherId
    };

    private static Course ToEntity(CourseModel m) => new()
    {
        Id = m.Id,
        Name = m.Name,
        Language = m.Language,
        Description = m.Description,
        Price = m.Price,
        LevelId = m.LevelId,
        TeacherId = m.TeacherId
    };
}