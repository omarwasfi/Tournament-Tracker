﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;

namespace TrackerLibrary.DataAccess
{
    public class TextConnection : IDataConnection
    {
        private const string PrizesFile = "PrizeModels.csv";
        private const string PeopleFile = "PersonModels.csv";
        private const string TeamFile = "TeamModels.csv";


        public PersonModel CreatePerson(PersonModel model)
        {
            List<PersonModel> people = PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            int currentId = 1;
            if (people.Count > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            people.Add(model);

            people.SaveToPersonFile(PeopleFile);

            return model;

        }

        // TODO - Wire up the createprize for the text file
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // Load the textfile and convert is to List<PrizeModel>
            List<PrizeModel> prizes =  PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            // Find the max ID
            int currentId = 1;
            if(prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }
             
            model.Id = currentId;
            
            // add the new record with the new id
            prizes.Add(model);


            // convert the prizes to list of string
            // save the list of string to the text file

            prizes.SaveToPrizeFile(PrizesFile);

            return model;
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(PeopleFile);

            int currentId = 1;
            if (teams.Count > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            teams.Add(model);

            teams.SaveToTeamFile(TeamFile);

            return model;
        }

        public List<PersonModel> GetPerson_All()
        {

            return PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }

        public List<TeamModel> GetTeam_All()
        {
            throw new NotImplementedException();
        }
    }
}
