using TestTask.Core.Models.Queries;

namespace TestTask.Core.Services;

public interface IMeteoriteService
{
    Task<List<MeteoriteSummaryResponse>> GetFilteredAsync(FilterValueRequest filters);
}
