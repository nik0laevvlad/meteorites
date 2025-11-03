namespace TestTask.Core.Models.Entities;

public class Geolocation
{
    public Geolocation() { }

    public string Type { get; set; } = null!;
    public Coordinates Coordinates { get; set; } = null!;

    public Geolocation(string type, double[] coordinates)
    {
        Type = type;
        Coordinates = new Coordinates(coordinates);
    }

    public Geolocation(string type, Coordinates coordinates)
    {
        Type = type;
        Coordinates = coordinates;
    }

    public Geolocation Clone() => new(Type, Coordinates.Clone());
}
