using Microsoft.AspNetCore.Mvc;
using TestTask.Core.DataAccess;
using TestTask.Core.Models.Queries;
using TestTask.Core.Services;

namespace TestTask.Api;

[ApiController]
[Route("api/meteorites")]
public class MeteoriteController : ControllerBase
{
    private readonly IMeteoriteRepository _meteoriteRepository;
    private readonly IMeteoriteService _meteoriteService;

    public MeteoriteController(IMeteoriteRepository meteoriteRepository, IMeteoriteService meteoriteService)
    {
        _meteoriteRepository = meteoriteRepository;
        _meteoriteService = meteoriteService;
    }

    [HttpGet]
    public async Task<List<MeteoriteSummaryResponse>> GetFilteredAsync([FromQuery] FilterValueRequest filterValue)
    {
        return await _meteoriteService.GetFilteredAsync(filterValue);
    }

    [HttpGet("classes")]
    public async Task<string[]> GetClassesAsync()
    {
        return await _meteoriteRepository.GetClassesAsync();
    }
}
