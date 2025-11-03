namespace TestTask.Core.Models.Entities;

public class Coordinates
{
    public Coordinates() { }

    public double Lon { get; set; }
    public double Lat { get; set; }

    public Coordinates(double lon, double lat)
    {
        Lon = lon;
        Lat = lat;
    }

    public Coordinates(double[] coordinates)
    {
        Lon = coordinates[0];
        Lat = coordinates[1];
    }

    public Coordinates Clone() => new(Lon, Lat);
}
