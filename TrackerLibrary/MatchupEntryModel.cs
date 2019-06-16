using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class MatchupEntryModel
    {

        /// <summary>
        /// Represents One team in the matchup
        /// </summary>
        public TeamModel TeamCompeting { get; set; }
        
        /// <summary>
        /// The score of this  team
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Represents the matchup that this team came From as the winner
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }
    }
}
