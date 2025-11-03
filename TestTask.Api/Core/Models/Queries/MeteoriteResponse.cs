namespace TestTask.Core.Models.Queries;

public class MeteoriteResponse
{
    public string? Name { get; set; }
    public string? NameType { get; set; }
    public string? RecClass { get; set; }
    public string? Fall { get; set; }
    public string? Id { get; set; }
    public string? Mass { get; set; }
    public string? Year { get; set; }
    public string? Reclat { get; set; }
    public string? Reclong { get; set; }
    public GeolocationResponse? Geolocation { get; set; }
}

public class GeolocationResponse
{
    public string? Type { get; set; }
    public double[]? Coordinates { get; set; }
}
