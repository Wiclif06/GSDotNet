using AgroOrbit.Api.Data;
using AgroOrbit.Api.Domain.Entities;
using AgroOrbit.Api.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroOrbit.Api.Controllers;

[ApiController]
[Route("api/sensors")]
public class SensorsController : ControllerBase
{
    private readonly AgroOrbitDbContext _db;
    public SensorsController(AgroOrbitDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Sensors.Include(s => s.Readings).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(SensorRequest request)
    {
        if (!await _db.CropAreas.AnyAsync(c => c.Id == request.CropAreaId))
            return BadRequest("Talhão não encontrado.");

        var sensor = new Sensor { Name = request.Name, SensorType = request.SensorType, CropAreaId = request.CropAreaId };
        _db.Sensors.Add(sensor);
        await _db.SaveChangesAsync();
        return Created($"/api/sensors/{sensor.Id}", sensor);
    }
}
