using System;
using AzureFunctions;
using DataBase.Models;
using DataBase.Services;
using DataBase.Services.Interface;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(StartUp))]
namespace AzureFunctions
{
    class StartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var connString = Environment.GetEnvironmentVariable("SqlConnectionString");
           
            builder.Services.AddDbContext<DBContext>(options => options
                    .UseSqlServer(connString ?? throw new ArgumentNullException(connString ,"Connection String is null"), item => item.EnableRetryOnFailure())
                    .UseSqlServer(o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                    .EnableSensitiveDataLogging()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
                ServiceLifetime.Transient);

            builder.Services.AddTransient<IGenericService<Customer>, CustomerService>();
            builder.Services.AddTransient<IGenericService<ProtectedZippedFiles>, ProtectedZippedFilesService>();
        }
    }
}
