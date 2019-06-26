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

        private const string db = "TournamentTrackerConnection";

        /// <summary>
        /// Saves a new prize to the database.
        /// </summary>
        /// <param name="model">The prize information</param>
        /// <returns>The proze information, incloding the unique identifier</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
          using (IDbConnection Connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
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

        public PersonModel CreatePerson(PersonModel model)
        {

            using (IDbConnection Connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@EmailAddress", model.EmailAddress);
                p.Add("@CellPhoneNumber", model.CellPhoneNumber);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                Connection.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@Id");
                return model;

            }
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            using (IDbConnection Connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@TeamName", model.TeamName);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                Connection.Execute("dbo.spTeams_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@Id");

                foreach (PersonModel tm in model.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@TeamId", model.Id);
                    p.Add("@PersonId", tm.Id);

                    Connection.Execute("dbo.spTeamMembers_Insert", p, commandType: CommandType.StoredProcedure);
                }





                return model;
            }
        }

        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> output;

            using (IDbConnection Connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = Connection.Query<PersonModel>("dbo.spPeople_GetAll").ToList();
            }
            return output;
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> output;

            using (IDbConnection Connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = Connection.Query<TeamModel>("dbo.spTeam_GetAll").ToList();
                foreach (TeamModel team in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@TeamId", team.Id);
                    team.TeamMembers = Connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam",p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            return output;
        }

        public void CreateTournament(TournamentModel model)
        {
            using (IDbConnection Connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                SaveTournament(Connection,model);

                SaveTournamentPrizes( Connection,  model);

                SaveTournamentEntries(Connection, model);

            }
        }
        private void SaveTournament(IDbConnection Connection,TournamentModel model)
        {
            
                var p = new DynamicParameters();
                p.Add("@TournamentName", model.TournamentName);
                p.Add("@EnrtyFee", model.EntryFee);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                Connection.Execute("dbo.spTournaments_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@Id");
            
        }

        private void SaveTournamentPrizes(IDbConnection Connection, TournamentModel model)
        {

            foreach (PrizeModel pz in model.Prizes)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@PrizeId", pz.Id);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                Connection.Execute("dbo.spTournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);
            }

        }
        private void SaveTournamentEntries(IDbConnection Connection, TournamentModel model)
        {

            foreach (TeamModel tm in model.EnteredTaems)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@TeamId", tm.Id);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                Connection.Execute("dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);
            }

        }
    }
}
