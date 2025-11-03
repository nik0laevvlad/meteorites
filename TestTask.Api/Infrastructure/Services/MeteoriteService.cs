using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using TestTask.Core.DataAccess;
using TestTask.Core.Models.Queries;
using TestTask.Core.Services;
using TestTask.Infrastructure.Options;

namespace TestTask.Infrastructure.Services;

public class MeteoriteService : IMeteoriteService
{
    private readonly IMeteoriteRepository _meteoriteRepository;
    private readonly IDistributedCache _cache;
    private readonly CacheOptions _options;

    public MeteoriteService(IMeteoriteRepository meteoriteRepository, IDistributedCache cache, IOptions<CacheOptions> options)
    {
        _meteoriteRepository = meteoriteRepository;
        _cache = cache;
        _options = options.Value;
    }

    public async Task<List<MeteoriteSummaryResponse>> GetFilteredAsync(FilterValueRequest filters)
    {
        var cacheKey = $"{filters.YearFrom}-{filters.YearTo}-{filters.RecClass}-{filters.Name}-{filters.SortField}-{filters.SortOrder}";

        var cached = await _cache.GetStringAsync(cacheKey);
        if (cached is not null)
            return JsonSerializer.Deserialize<List<MeteoriteSummaryResponse>>(cached)!;

        var data = await _meteoriteRepository.GetFilteredAsync(filters);

        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(data),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromMinutes(_options.SummaryTtlMinutes)
            });

        return data;
    }
}
