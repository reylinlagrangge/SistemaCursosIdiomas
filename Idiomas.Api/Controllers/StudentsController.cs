using Idiomas.Domain.Entities;
using Idiomas.Infrastructure.Exceptions;
using Idiomas.Infrastructure.Interfaces;
using Idiomas.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Idiomas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentRepository _repository;

    public StudentsController(IStudentRepository repository) => _repository = repository;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _repository.GetAllAsync();
        return Ok(students.Select(ToModel));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _repository.GetByIdAsync(id);
        return student is null ? NotFound() : Ok(ToModel(student));
    }

    [HttpPost]
    public async Task<IActionResult> Create(StudentModel model)
    {
        try
        {
            var created = await _repository.AddAsync(ToEntity(model));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToModel(created));
        }
        catch (StudentException ex) { return Conflict(ex.Message); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, StudentModel model)
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

    private static StudentModel ToModel(Student s) => new()
    {
        Id = s.Id,
        FirstName = s.FirstName,
        LastName = s.LastName,
        Email = s.Email,
        Phone = s.Phone,
        StudentNumber = s.StudentNumber
    };

    private static Student ToEntity(StudentModel m) => new()
    {
        Id = m.Id,
        FirstName = m.FirstName,
        LastName = m.LastName,
        Email = m.Email,
        Phone = m.Phone,
        StudentNumber = m.StudentNumber
    };
}