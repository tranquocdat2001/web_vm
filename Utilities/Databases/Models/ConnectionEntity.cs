using System.ComponentModel;

namespace Utilities.Databases.Models
{
    public class ConnectionEntity
    {
        public enum ConnectionStatus
        {
            Closed = 0,
            Open = 1,
            Connecting = 2,
            Executing = 4,
            Fetching = 8,
            Broken = 16,
            NoConnected = 9999
        }

        public enum DBPosition
        {
            Manual = -1,
            [Description("ConnectionString")]
            Default = 0,
            [Description("MasterConnectionString")]
            Master = 1,
            [Description("SlaveConnectionString")]
            Slave = 2,
            [Description("MySqlConnectionString")]
            MySql = 3,

        }
    }
}
