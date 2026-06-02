using AgroOrbit.Api.Data;
using AgroOrbit.Api.Domain.Entities;
using AgroOrbit.Api.Domain.Enums;
using AgroOrbit.Api.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroOrbit.Api.Controllers;

[ApiController]
[Route("api/crop-areas")]
public class CropAreasController : ControllerBase
{
    private readonly AgroOrbitDbContext _db;
    public CropAreasController(AgroOrbitDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.CropAreas.OrderBy(c => c.Id).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var cropArea = await _db.CropAreas
            .Include(c => c.Sensors)
            .Include(c => c.SatelliteData)
            .Include(c => c.ClimateAlerts)
            .FirstOrDefaultAsync(c => c.Id == id);

        return cropArea is null ? NotFound() : Ok(cropArea);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CropAreaRequest request)
    {
        if (!await _db.Farms.AnyAsync(f => f.Id == request.FarmId))
            return BadRequest("Fazenda não encontrada.");

        var cropArea = new CropArea
        {
            Name = request.Name,
            CropType = request.CropType,
            AreaSize = request.AreaSize,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            FarmId = request.FarmId,
            Status = CropStatus.NORMAL
        };

        _db.CropAreas.Add(cropArea);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = cropArea.Id }, cropArea);
    }
}
