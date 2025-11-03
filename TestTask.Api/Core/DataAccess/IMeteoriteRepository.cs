using TestTask.Core.Models.Entities;
using TestTask.Core.Models.Queries;

namespace TestTask.Core.DataAccess;

public interface IMeteoriteRepository
{
    Task AddRangeAsync(IEnumerable<Meteorite> meteorites, CancellationToken cancellationToken = default);
    void DeleteRange(IEnumerable<Meteorite> meteorites);
    Task<List<Meteorite>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<string[]> GetClassesAsync();
    Task<List<MeteoriteSummaryResponse>> GetFilteredAsync(FilterValueRequest query);
}
