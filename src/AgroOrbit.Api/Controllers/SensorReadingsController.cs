using AgroOrbit.Api.Data;
using AgroOrbit.Api.Domain.Entities;
using AgroOrbit.Api.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroOrbit.Api.Controllers;

[ApiController]
[Route("api/sensor-readings")]
public class SensorReadingsController : ControllerBase
{
    private readonly AgroOrbitDbContext _db;
    public SensorReadingsController(AgroOrbitDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.SensorReadings.OrderByDescending(r => r.ReadingDate).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(SensorReadingRequest request)
    {
        if (!await _db.Sensors.AnyAsync(s => s.Id == request.SensorId))
            return BadRequest("Sensor não encontrado.");

        var reading = new SensorReading
        {
            SensorId = request.SensorId,
            Temperature = request.Temperature,
            AirHumidity = request.AirHumidity,
            SoilHumidity = request.SoilHumidity,
            ManualAlert = request.ManualAlert
        };

        _db.SensorReadings.Add(reading);
        await _db.SaveChangesAsync();
        return Created($"/api/sensor-readings/{reading.Id}", reading);
    }
}
