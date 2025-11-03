using Microsoft.Extensions.Options;
using TestTask.Core.DataAccess;
using TestTask.Core.Models.Entities;
using TestTask.Core.Models.Queries;
using TestTask.Core.Services;
using TestTask.Infrastructure.Options;

namespace TestTask.Infrastructure.Services;

public class MeteoriteSyncService : IMeteoriteSyncService
{
    private readonly HttpClient _http;
    private readonly IMeteoriteRepository _meteoriteRepository;
    private readonly ILogger<MeteoriteSyncService> _logger;
    private readonly AppDbContext _dbContext;
    private readonly ExternalApiOptions _options;

    public MeteoriteSyncService(HttpClient http, IMeteoriteRepository meteoriteRepository, ILogger<MeteoriteSyncService> logger, AppDbContext dbContext, IOptions<ExternalApiOptions> options)
    {
        _http = http;
        _meteoriteRepository = meteoriteRepository;
        _logger = logger;
        _dbContext = dbContext;
        _options = options.Value;
    }

    public async Task SyncAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching data");

        var response = await _http.GetFromJsonAsync<List<MeteoriteResponse>>(_options.MeteoriteUrl, cancellationToken);

        if (response == null)
        {
            _logger.LogWarning("Remote data is empty");
            return;
        }

        var mapped = response.Select(x => Meteorite.FromResponse(x)).ToList();

        await ApplyDiffAsync(mapped, cancellationToken);
    }

    private async Task ApplyDiffAsync(List<Meteorite> newItems, CancellationToken ct)
    {
        var oldItems = await _meteoriteRepository.GetAllAsync(ct);

        var oldMap = oldItems.ToDictionary(x => x.Id);
        var newMap = newItems.ToDictionary(x => x.Id);

        var toAdd = newItems
            .Where(x => !oldMap.ContainsKey(x.Id))
            .ToList();

        if (toAdd.Count > 0)
            await _meteoriteRepository.AddRangeAsync(toAdd, ct);

        foreach (var item in newItems.Where(x => oldMap.ContainsKey(x.Id)))
        {
            var existing = oldMap[item.Id];
            existing.Update(item);
        }

        var toDelete = oldItems
            .Where(x => !newMap.ContainsKey(x.Id))
            .ToList();

        if (toDelete.Count > 0)
            _meteoriteRepository.DeleteRange(toDelete);

        await _dbContext.SaveChangesAsync(ct);
    }
}
