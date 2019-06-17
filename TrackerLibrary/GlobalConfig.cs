using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;
using System.Configuration;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnection Connection { get; private set; } 


        public static void InitializeConnections(DataBaseType DBType)
        {
            
            if (DBType == DataBaseType.Sql)
            {
                // TODO - Set up the SQL connecor properly
                SqlConnector Sql = new SqlConnector();
                Connection = Sql;
            }
            else if (DBType == DataBaseType.TextFile)
            {
                // TODO - Create the text Connection
                TextConnection Text = new TextConnection();
                Connection = Text;
            }
            
        }
        public static string CnnString(string Name)
        {
            return ConfigurationManager.ConnectionStrings[Name].ConnectionString;
        }
    }
}
