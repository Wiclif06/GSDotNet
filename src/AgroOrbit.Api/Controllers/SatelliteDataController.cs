using AgroOrbit.Api.Data;
using AgroOrbit.Api.Domain.Entities;
using AgroOrbit.Api.DTOs.Requests;
using AgroOrbit.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroOrbit.Api.Controllers;

[ApiController]
[Route("api/satellite-data")]
public class SatelliteDataController : ControllerBase
{
    private readonly AgroOrbitDbContext _db;
    private readonly RiskAnalysisService _risk;

    public SatelliteDataController(AgroOrbitDbContext db, RiskAnalysisService risk)
    {
        _db = db;
        _risk = risk;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.SatelliteData.OrderByDescending(s => s.CaptureDate).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(SatelliteDataRequest request)
    {
        var cropArea = await _db.CropAreas.FindAsync(request.CropAreaId);
        if (cropArea is null) return BadRequest("Talhão não encontrado.");

        var data = new SatelliteData
        {
            CropAreaId = request.CropAreaId,
            Source = request.Source,
            NdviAverage = request.NdviAverage,
            NdviMin = request.NdviMin,
            NdviMax = request.NdviMax,
            SurfaceTemperature = request.SurfaceTemperature,
            CloudCoverage = request.CloudCoverage,
            FireFocusNearby = request.FireFocusNearby,
            FireFocusDistanceKm = request.FireFocusDistanceKm,
            CaptureDate = request.CaptureDate
        };

        cropArea.Status = _risk.CalculateCropStatus(data.NdviAverage, data.SurfaceTemperature, data.CloudCoverage, data.FireFocusNearby);

        var alert = _risk.GenerateAlertIfNeeded(cropArea, data);
        if (alert is not null)
        {
            _db.ClimateAlerts.Add(alert);
            _db.Recommendations.Add(new Recommendation
            {
                ClimateAlert = alert,
                Priority = alert.Severity.ToString(),
                Message = "Verificar o talhão e considerar irrigação ou inspeção local conforme o nível de risco."
            });
        }

        _db.SatelliteData.Add(data);
        await _db.SaveChangesAsync();

        return Created($"/api/satellite-data/{data.Id}", data);
    }
}
