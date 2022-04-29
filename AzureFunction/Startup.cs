using System;
using AzureFunctions;
using AzureFunctions.FunctionServices;
using AzureFunctions.FunctionServices.Interface;
using DataBase.DataBaseServices;
using DataBase.DataBaseServices.Interface;
using DataBase.Models;
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

            builder.Services.AddTransient<IGenericDataBaseService<Customer>, DataBaseCustomerService>();
            builder.Services.AddTransient<IGenericDataBaseService<ZippedFiles>, DataBaseZippedFilesService>();
            builder.Services.AddTransient<IZippedFilesFunctionService, ZippedFilesFunctionService>();
        }
    }
}
