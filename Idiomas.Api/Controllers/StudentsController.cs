using Idiomas.Application.Contract;
using Idiomas.Application.Dtos;
using Idiomas.Application.Dtos.Students;
using Microsoft.AspNetCore.Mvc;

namespace Idiomas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service) => _service = service;

    private IActionResult ToActionResult<T>(ServiceResult<T> result) =>
        result.Success ? Ok(result.Data)
        : result.NotFound ? NotFound(result.Message)
        : BadRequest(result.Message);

    [HttpGet]
    public async Task<IActionResult> GetAll() => ToActionResult(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => ToActionResult(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(SaveStudentDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return result.Success
            ? CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data)
            : ToActionResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SaveStudentDto dto) =>
        ToActionResult(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result.Success ? NoContent() : ToActionResult(result);
    }
}
