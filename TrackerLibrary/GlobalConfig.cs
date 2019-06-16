using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();

        public static void InitializeConnections(bool Database, bool TextFiles)
        {
            if (Database)
            {
                // TODO - Set up the SQL connecor properly
                SqlConnector Sql = new SqlConnector();
                Connections.Add(Sql);
            }
            if (TextFiles)
            {
                // TODO - Create the text Connection
                TextConnection Text = new TextConnection();
                Connections.Add(Text);
            }
            
        }
    }
}
