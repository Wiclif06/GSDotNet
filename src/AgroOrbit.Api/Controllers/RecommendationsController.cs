using AgroOrbit.Api.Data;
using AgroOrbit.Api.Domain.Entities;
using AgroOrbit.Api.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroOrbit.Api.Controllers;

[ApiController]
[Route("api/recommendations")]
public class RecommendationsController : ControllerBase
{
    private readonly AgroOrbitDbContext _db;
    public RecommendationsController(AgroOrbitDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Recommendations.OrderByDescending(r => r.CreatedAt).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(RecommendationRequest request)
    {
        var recommendation = new Recommendation
        {
            ClimateAlertId = request.ClimateAlertId,
            Message = request.Message,
            Priority = request.Priority
        };

        _db.Recommendations.Add(recommendation);
        await _db.SaveChangesAsync();
        return Created($"/api/recommendations/{recommendation.Id}", recommendation);
    }
}
