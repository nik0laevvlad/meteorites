namespace TestTask.Core.Services;

public interface IMeteoriteSyncService
{
    Task SyncAsync(CancellationToken cancellationToken);
}
