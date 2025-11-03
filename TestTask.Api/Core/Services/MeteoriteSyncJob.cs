using Quartz;

namespace TestTask.Core.Services;

public class MeteoriteSyncJob : IJob
{
    private readonly ILogger<MeteoriteSyncJob> _logger;
    private readonly IMeteoriteSyncService _syncService;

    public MeteoriteSyncJob(ILogger<MeteoriteSyncJob> logger, IMeteoriteSyncService syncService)
    {
        _logger = logger;
        _syncService = syncService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Sync started");
            await _syncService.SyncAsync(context.CancellationToken);
            _logger.LogInformation("Synchronization is finished");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sync failed");
        }
    }
}
