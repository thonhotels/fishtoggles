using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Thon.Hotels.FishToggles
{
    public class TogglesConfigProvider : ConfigurationProvider
    {
        private string ConnectionString { get; }
        private Action<Exception, string> LogException { get; }

        public TogglesConfigProvider(string connectionString, Action<Exception, string> logException)
        {
            ConnectionString = connectionString;
            LogException = logException;
        }

        public override void Load()
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    Data = GetValues(conn).ToDictionary(c => c.Id, c => c.Value);
                }
            }
            catch (SqlException ex)
            {
                //Log.Warning(ex, "Failed to read feature toggles from database");
                LogException(ex, "Failed to read feature toggles from database");
                Data = new Dictionary<string, string>();
            }
        }
        private IEnumerable<ConfigurationValue> GetValues(SqlConnection connection)
        {
            var cmd = new SqlCommand("SELECT [Id], [Value] FROM Toggles;", connection);
            var reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    yield return new ConfigurationValue { Id = reader.GetString(0), Value = reader.GetString(1) };
                }
            }
            reader.Close();
        }
    }
}
