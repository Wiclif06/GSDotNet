using AgroOrbit.Api.Data;
using AgroOrbit.Api.Domain.Entities;
using AgroOrbit.Api.Domain.Enums;
using AgroOrbit.Api.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroOrbit.Api.Controllers;

[ApiController]
[Route("api/climate-alerts")]
public class ClimateAlertsController : ControllerBase
{
    private readonly AgroOrbitDbContext _db;
    public ClimateAlertsController(AgroOrbitDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.ClimateAlerts.Include(a => a.Recommendations).OrderByDescending(a => a.CreatedAt).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(ClimateAlertRequest request)
    {
        if (!Enum.TryParse<AlertSeverity>(request.Severity, true, out var severity))
            return BadRequest("Severidade inválida. Use LOW, MEDIUM, HIGH ou CRITICAL.");

        var alert = new ClimateAlert
        {
            CropAreaId = request.CropAreaId,
            AlertType = request.AlertType,
            Title = request.Title,
            Description = request.Description,
            Severity = severity
        };

        _db.ClimateAlerts.Add(alert);
        await _db.SaveChangesAsync();
        return Created($"/api/climate-alerts/{alert.Id}", alert);
    }

    [HttpPut("{id:int}/resolve")]
    public async Task<IActionResult> Resolve(int id)
    {
        var alert = await _db.ClimateAlerts.FindAsync(id);
        if (alert is null) return NotFound();

        alert.Status = AlertStatus.RESOLVED;
        alert.ResolvedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }
}
