using System;
using Microsoft.Extensions.Configuration;

namespace Thon.Hotels.FishToggles
{
    public class SqlConfigSource : IConfigurationSource
    {
        private string ConnectionString { get; }
        private Action<Exception, string> LogException { get; }

        public SqlConfigSource(string connectionString, Action<Exception, string> logException)
        {
            ConnectionString = connectionString;
            LogException = logException;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder) => new TogglesConfigProvider(ConnectionString, LogException);
    }
}
