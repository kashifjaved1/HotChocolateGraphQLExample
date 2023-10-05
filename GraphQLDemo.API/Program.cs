using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace GraphQLDemo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configured Serilog
            // configured serilog below.
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                    path: ".\\debugLogs\\log-.txt",
                    outputTemplate: "{Timestamp: yyyy-MM-dd HH:mm:ss.fff zzz} [{Level: u3}] {Message: lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information
                ).CreateLogger();

            //IHost host = CreateHostBuilder(args).Build();

            //using (IServiceScope scope = host.Services.CreateScope())
            //{
            //    IDbContextFactory<SchoolDbContext> contextFactory =
            //        scope.ServiceProvider.GetRequiredService<IDbContextFactory<SchoolDbContext>>();

            //    using (SchoolDbContext context = contextFactory.CreateDbContext())
            //    {
            //        context.Database.Migrate();
            //    }
            //}

            //host.Run();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
