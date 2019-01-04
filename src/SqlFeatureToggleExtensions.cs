using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Thon.Hotels.FishToggles
{
    public static class SqlFeatureToggleExtensions
    {
        public static IConfigurationBuilder AddSqlFeatureToggleConfig(this IConfigurationBuilder builder, string connectionString, Action<Exception, string> logException) =>
            builder.Add(new SqlConfigSource(connectionString, logException));

        public static IWebHostBuilder UseFeatureToggles(this IWebHostBuilder hostBuilder, string[] commandLineArgs, Action<Exception, string> logException, string connectionStringName = "DefaultConnection")
        {
            IConfigurationRoot GetConnectionStringConfiguration(Assembly asm)
            {
                    var builder = new ConfigurationBuilder()
                                                .AddCommandLine(commandLineArgs)
                                                .SetBasePath(Directory.GetCurrentDirectory())
                                                .AddJsonFile("appsettings.json");
                    
                    if (IsDevelopment())
                        builder = builder.AddUserSecrets(assembly:asm, optional:true);

                    builder = builder.AddEnvironmentVariables();
                    return builder.Build();
            }

            bool IsDevelopment() =>
                string.Equals(
                        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                        EnvironmentName.Development,
                        StringComparison.OrdinalIgnoreCase);

            var assembly = Assembly.GetCallingAssembly();
            return hostBuilder.ConfigureAppConfiguration((hostingContext, cfg) =>
                {
                    var connectionString = GetConnectionStringConfiguration(assembly).GetConnectionString(connectionStringName);
                    if (!string.IsNullOrEmpty(connectionString) && !connectionString.StartsWith("Dummy"))
                        cfg.AddSqlFeatureToggleConfig(connectionString, logException);
                });
        }
    }
}
