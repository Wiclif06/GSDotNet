using AgroOrbit.Api.Data;
using AgroOrbit.Api.Domain.Entities;
using AgroOrbit.Api.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroOrbit.Api.Controllers;

[ApiController]
[Route("api/farms")]
public class FarmsController : ControllerBase
{
    private readonly AgroOrbitDbContext _db;
    public FarmsController(AgroOrbitDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Farms.Include(f => f.CropAreas).OrderBy(f => f.Id).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var farm = await _db.Farms.Include(f => f.CropAreas).FirstOrDefaultAsync(f => f.Id == id);
        return farm is null ? NotFound() : Ok(farm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(FarmRequest request)
    {
        var farm = new Farm
        {
            Name = request.Name,
            OwnerName = request.OwnerName,
            City = request.City,
            State = request.State.ToUpper(),
            TotalArea = request.TotalArea,
            UserId = request.UserId
        };

        _db.Farms.Add(farm);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = farm.Id }, farm);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, FarmRequest request)
    {
        var farm = await _db.Farms.FindAsync(id);
        if (farm is null) return NotFound();

        farm.Name = request.Name;
        farm.OwnerName = request.OwnerName;
        farm.City = request.City;
        farm.State = request.State.ToUpper();
        farm.TotalArea = request.TotalArea;
        farm.UserId = request.UserId;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var farm = await _db.Farms.FindAsync(id);
        if (farm is null) return NotFound();

        _db.Farms.Remove(farm);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
