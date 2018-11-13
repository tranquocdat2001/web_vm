using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using FluentData;

namespace Utilities
{
    public abstract class ContextBase
    {
        protected enum DBPosition
        {
            Manual = -1,
            [Description("ConnectionString")]
            Default = 0,
            [Description("MasterConnection")]
            Master = 1,
            [Description("SlaveConnection")]
            Slave = 2,
            [Description("ExternalConnection")]
            External = 3,
            [Description("CrawlerBankConnection")]
            CrawlerBank = 4,
            [Description("CrawlerConnection")]
            Crawler = 5,
            [Description("FaqConnection")]
            Faq = 6,
            [Description("BCRMConnection")]
            BCRM = 7
        }

        protected DBPosition _dbPosition = DBPosition.Slave;

        protected IDbContext Context()
        {
            return new DbContext().ConnectionString(GetConnectionString(_dbPosition), new SqlServerProvider());
        }

        protected IDbContext Context(DBPosition dbPosition, string manualConnection = "")
        {
            return new DbContext().ConnectionString(GetConnectionString(dbPosition, manualConnection), new SqlServerProvider());
        }

        protected string GetConnectionString(DBPosition dbPosition, string defaultConnectionName = "ConnectionString")
        {
            string connectionName = string.Empty;
            if (dbPosition == DBPosition.Manual)
            {
                if (!string.IsNullOrEmpty(defaultConnectionName)) connectionName = defaultConnectionName;
            }
            else
            {
                connectionName = StringUtils.GetEnumDescription(dbPosition); 
            }

            if (string.IsNullOrEmpty(connectionName)) connectionName = "ConnectionString";

            return AppSettings.Instance.GetConnection(connectionName);
        }

        protected void BulkInsert(DataTable dataTable, string tableName)
        {
            if (dataTable == null || dataTable.Rows.Count <= 0)
            {
                throw new Exception("Data table not allow null");
            }

            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception("Table's name not allow null");
            }

            using (SqlBulkCopy bulk = new SqlBulkCopy(GetConnectionString(_dbPosition)))
            {
                bulk.BatchSize = dataTable.Rows.Count;
                bulk.BulkCopyTimeout = 60;
                bulk.DestinationTableName = tableName;

                bulk.WriteToServer(dataTable);

                bulk.Close();
            }
        }

        protected void BulkInsert(DataTable dataTable, string tableName, string connectionName)
        {
            if (dataTable == null || dataTable.Rows.Count <= 0)
            {
                throw new Exception("Data table not allow null");
            }

            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception("Table's name not allow null");
            }

            using (SqlBulkCopy bulk = new SqlBulkCopy(GetConnectionString(DBPosition.Manual, connectionName)))
            {
                bulk.BatchSize = dataTable.Rows.Count;
                bulk.BulkCopyTimeout = 60;
                bulk.DestinationTableName = tableName;

                bulk.WriteToServer(dataTable);

                bulk.Close();
            }
        }

        protected void BulkUpdateAndCommand(DataTable dataTable, string tableName, string firstCommand, string backCommand)
        {
            if (dataTable == null || dataTable.Rows.Count <= 0)
            {
                throw new Exception("Data table not allow null");
            }

            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception("Table's name not allow null");
            }
            using (SqlConnection conn = new SqlConnection(GetConnectionString(_dbPosition)))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(firstCommand, conn);
                command.ExecuteNonQuery();
                
                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {
                    bulk.BatchSize = dataTable.Rows.Count;
                    bulk.BulkCopyTimeout = 60;
                    bulk.DestinationTableName = tableName;

                    bulk.WriteToServer(dataTable);

                    bulk.Close();
                }

                command = new SqlCommand(backCommand, conn);
                command.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
