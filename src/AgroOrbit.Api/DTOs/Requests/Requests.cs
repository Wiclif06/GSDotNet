using System.ComponentModel.DataAnnotations;

namespace AgroOrbit.Api.DTOs.Requests;

public record UserRequest(
    [Required, StringLength(120)] string Name,
    [Required, EmailAddress, StringLength(160)] string Email,
    [Required, StringLength(40)] string Role
);

public record FarmRequest(
    [Required, StringLength(120)] string Name,
    [Required, StringLength(120)] string OwnerName,
    [Required, StringLength(80)] string City,
    [Required, StringLength(2, MinimumLength = 2)] string State,
    [Range(0.1, 999999)] decimal TotalArea,
    int? UserId
);

public record CropAreaRequest(
    [Required, StringLength(120)] string Name,
    [Required, StringLength(80)] string CropType,
    [Range(0.1, 999999)] decimal AreaSize,
    [Range(-90, 90)] decimal Latitude,
    [Range(-180, 180)] decimal Longitude,
    [Required] int FarmId
);

public record SensorRequest(
    [Required, StringLength(120)] string Name,
    [Required, StringLength(60)] string SensorType,
    [Required] int CropAreaId
);

public record SensorReadingRequest(
    [Required] int SensorId,
    [Range(-30, 80)] decimal Temperature,
    [Range(0, 100)] decimal AirHumidity,
    [Range(0, 100)] decimal SoilHumidity,
    bool ManualAlert
);

public record SatelliteDataRequest(
    [Required] int CropAreaId,
    [Required, StringLength(80)] string Source,
    [Range(-1, 1)] decimal NdviAverage,
    [Range(-1, 1)] decimal NdviMin,
    [Range(-1, 1)] decimal NdviMax,
    [Range(-50, 90)] decimal SurfaceTemperature,
    [Range(0, 100)] decimal CloudCoverage,
    bool FireFocusNearby,
    decimal? FireFocusDistanceKm,
    DateOnly CaptureDate
);

public record ClimateAlertRequest(
    [Required] int CropAreaId,
    [Required, StringLength(60)] string AlertType,
    [Required, StringLength(160)] string Title,
    [Required, StringLength(500)] string Description,
    [Required] string Severity
);

public record RecommendationRequest(
    [Required] int ClimateAlertId,
    [Required, StringLength(500)] string Message,
    [Required, StringLength(30)] string Priority
);
