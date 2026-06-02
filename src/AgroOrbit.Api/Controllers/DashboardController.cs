using AgroOrbit.Api.Data;
using AgroOrbit.Api.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgroOrbit.Api.Domain.Enums;

namespace AgroOrbit.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly AgroOrbitDbContext _db;
    public DashboardController(AgroOrbitDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var averageNdvi = await _db.SatelliteData.AnyAsync()
            ? await _db.SatelliteData.AverageAsync(s => s.NdviAverage)
            : 0;

        return Ok(new DashboardResponse(
            await _db.Users.CountAsync(),
            await _db.Farms.CountAsync(),
            await _db.CropAreas.CountAsync(),
            await _db.Sensors.CountAsync(),
            await _db.SatelliteData.CountAsync(),
            await _db.ClimateAlerts.CountAsync(a => a.Status == AlertStatus.OPEN),
            await _db.Recommendations.CountAsync(),
            averageNdvi
        ));
    }
}
