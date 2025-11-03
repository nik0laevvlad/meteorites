using System.Globalization;
using TestTask.Core.Models.Queries;

namespace TestTask.Core.Models.Entities;

public class Meteorite
{
    public string Name { get; protected set; } = null!;
    public int Id { get; protected set; }
    public NameType? NameType { get; protected set; }
    public string? RecClass { get; protected set; } = null!;
    public double? Mass { get; protected set; }
    public FallType? Fall { get; protected set; }
    public DateTime? Year { get; protected set; }
    public double? RecLat { get; protected set; }
    public double? RecLong { get; protected set; }
    public Geolocation? Geolocation { get; protected set; }

    public void Update(Meteorite value)
    {
        Name = value.Name;
        NameType = value.NameType;
        RecClass = value.RecClass;
        Mass = value.Mass;
        Fall = value.Fall;
        Year = value.Year;
        RecLat = value.RecLat;
        RecLong = value.RecLong;

        Geolocation = value.Geolocation != null ? value.Geolocation.Clone() : null;
    }

    public static Meteorite FromResponse(MeteoriteResponse response)
    {
        return new Meteorite
        {
            Name = response.Name,
            Id = int.Parse(response.Id),
            NameType = Enum.Parse<NameType>(response.NameType),
            RecClass = response.RecClass,
            Mass = double.TryParse(response.Mass, CultureInfo.InvariantCulture, out var mass) ? mass : null,
            Fall = Enum.Parse<FallType>(response.Fall),
            Year = DateTime.TryParse(response.Year, out var year) ? year : null,
            RecLat = double.TryParse(response.Reclat, CultureInfo.InvariantCulture, out var recLat) ? recLat : null,
            RecLong = double.TryParse(response.Reclong, CultureInfo.InvariantCulture, out var recLong) ? recLong : null,

            Geolocation = response.Geolocation != null ? new Geolocation(response.Geolocation.Type, response.Geolocation.Coordinates) : null
        };
    }
}
