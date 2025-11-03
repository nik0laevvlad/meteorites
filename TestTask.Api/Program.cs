using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Quartz;
using TestTask.Core;
using TestTask.Core.DataAccess;
using TestTask.Core.Services;
using TestTask.Infrastructure;
using TestTask.Infrastructure.Options;
using TestTask.Infrastructure.Repositories;
using TestTask.Infrastructure.Services;
using QuartzOptions = TestTask.Infrastructure.Options.QuartzOptions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<CacheOptions>(configuration.GetSection("Cache"));
builder.Services.Configure<QuartzOptions>(configuration.GetSection("Quartz"));
builder.Services.Configure<ExternalApiOptions>(configuration.GetSection("ExternalApi"));

builder.Services
    .AddControllers();

builder.Services
    .AddStackExchangeRedisCache(opt =>
    {
        opt.Configuration = configuration.GetConnectionString("Redis");
    });

builder.Services
    .AddSwaggerGen()
    .AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
    .AddHttpClient<IMeteoriteSyncService, MeteoriteSyncService>()
    .AddPolicyHandler(PollyPolicies.GetRetryPolicy());

builder.Services.AddValidatorsFromAssemblyContaining<FilterValueRequestValidator>();


builder.Services
    .AddScoped<IMeteoriteRepository, MeteoriteRepositoryAdapter>()
    .AddScoped<IMeteoriteService, MeteoriteService>();

builder.Services.AddQuartz(q =>
{
    var cron = configuration["Quartz:SyncCron"];
    var jobKey = new JobKey("SyncMeteorites");

    q.AddJob<MeteoriteSyncJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("SyncMeteorites-trigger")
        .WithCronSchedule(cron)
    );
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
