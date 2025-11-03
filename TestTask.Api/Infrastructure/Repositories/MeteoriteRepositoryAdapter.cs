using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using TestTask.Core.DataAccess;
using TestTask.Core.Models.Entities;
using TestTask.Core.Models.Queries;

namespace TestTask.Infrastructure.Repositories;

public class MeteoriteRepositoryAdapter : IMeteoriteRepository
{
    private readonly AppDbContext _dbContext;

    public MeteoriteRepositoryAdapter(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddRangeAsync(IEnumerable<Meteorite> meteorites, CancellationToken cancellationToken = default)
    {
        await _dbContext.BulkInsertAsync(meteorites, cancellationToken: cancellationToken);
    }

    public void DeleteRange(IEnumerable<Meteorite> meteorites)
    {
        _dbContext.Meteorites.RemoveRange(meteorites);
    }

    public async Task<List<Meteorite>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Meteorites
           .Include(x => x.Geolocation)
           .ThenInclude(g => g.Coordinates)
           .ToListAsync(cancellationToken);
    }

    public async Task<string[]> GetClassesAsync()
    {
        return await _dbContext.Meteorites
            .AsNoTracking()
            .Where(x => x.RecClass != null)
            .Select(m => m.RecClass!)
            .Distinct()
            .OrderBy(c => c)
            .ToArrayAsync();
    }

    public async Task<List<MeteoriteSummaryResponse>> GetFilteredAsync(FilterValueRequest query)
    {
        if (query.YearFrom.HasValue && query.YearTo.HasValue && query.YearFrom > query.YearTo)
            throw new ArgumentException("YearTo must be greater than YearFrom");

        var queryable = _dbContext.Meteorites.AsNoTracking().AsQueryable();

        if (query.YearFrom.HasValue)
            queryable = queryable.Where(m => m.Year.HasValue && m.Year >= query.YearFrom.Value);

        if (query.YearTo.HasValue)
            queryable = queryable.Where(m => m.Year.HasValue && m.Year <= query.YearTo.Value);

        if (!string.IsNullOrWhiteSpace(query.RecClass))
            queryable = queryable.Where(m => m.RecClass == query.RecClass);

        if (!string.IsNullOrWhiteSpace(query.Name))
            queryable = queryable.Where(m => m.Name.Contains(query.Name));

        var groupedQuery = queryable
           .Where(x => x.Year != null)
           .GroupBy(m => m.Year!.Value.Year)
           .Select(g => new MeteoriteSummaryResponse
           {
               Year = g.Key,
               Count = g.Count(),
               TotalMass = g.Sum(m => m.Mass ?? 0)
           });

        groupedQuery = query.SortField switch
        {
            "year" => query.SortOrder == "desc"
                ? groupedQuery.OrderByDescending(g => g.Year)
                : groupedQuery.OrderBy(g => g.Year),

            "count" => query.SortOrder == "desc"
                ? groupedQuery.OrderByDescending(g => g.Count)
                : groupedQuery.OrderBy(g => g.Count),

            "totalMass" => query.SortOrder == "desc"
                ? groupedQuery.OrderByDescending(g => g.TotalMass)
                : groupedQuery.OrderBy(g => g.TotalMass),

            _ => groupedQuery.OrderBy(g => g.Year)
        };

        return await groupedQuery.ToListAsync();
    }
}
