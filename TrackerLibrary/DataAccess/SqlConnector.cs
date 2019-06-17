using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using System.Data;
using Dapper;



namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        
        /// <summary>
        /// Saves a new prize to the database.
        /// </summary>
        /// <param name="model">The prize information</param>
        /// <returns>The proze information, incloding the unique identifier</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
          using (IDbConnection Connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("TournamentTrackerConnection")))
            {
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PriceAmount", model.PrizeAmount);
                p.Add("@PricePercentage", model.PrizePercentage);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                Connection.Execute("dbo.spPrizes_Insert",p,commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@Id");
                return model;

            }

        }
    }
}
