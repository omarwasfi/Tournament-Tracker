using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary.Models;
using TrackerLibrary;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        List<TeamModel> availbleTeams = GlobalConfig.Connection.GetTeam_All();
        List<TeamModel> selectedTeams = new List<TeamModel>();
        List<PrizeModel> selectedPrizes = new List<PrizeModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();

            WireUpLists();
        }

        private void WireUpLists()
        {
            SelectTeamDropDown.DataSource = null;
            SelectTeamDropDown.DataSource = availbleTeams;
            SelectTeamDropDown.DisplayMember = "TeamName";


            TournamentTeamsListBox.DataSource = null;
            TournamentTeamsListBox.DataSource = selectedTeams;
            TournamentTeamsListBox.DisplayMember = "TeamName";


            PrizesListBox.DataSource = null;
            PrizesListBox.DataSource = selectedPrizes;
            PrizesListBox.DisplayMember = "PlaceName";
        }

        private void AddTeamButton_Click(object sender, EventArgs e)
        {
            if (SelectTeamDropDown.SelectedItem != null)
            {
                TeamModel t = (TeamModel)SelectTeamDropDown.SelectedItem;

                availbleTeams.Remove(t);
                selectedTeams.Add(t);


                WireUpLists();
            }
        }

        private void CreatePrizeButton_Click(object sender, EventArgs e)
        {
            // Call the create prize form
            CreatePrizeForm frm = new CreatePrizeForm(this);
            frm.Show();

            
        }

        public void PrizeComplete(PrizeModel model)
        {
            // get back from the form prize model
            // take that prize model and put it into our list selected prize
            selectedPrizes.Add(model);
            WireUpLists();
        }

        public void TeamComplete(TeamModel model)
        {
            selectedTeams.Add(model);
            WireUpLists();
        }

        private void CreateNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();
        }

        private void RemoveSelectedPlayerButton_Click(object sender, EventArgs e)
        {
            if (TournamentTeamsListBox.SelectedItem != null)
            {
                TeamModel t = (TeamModel)TournamentTeamsListBox.SelectedItem;

                selectedTeams.Remove(t);
                availbleTeams.Add(t);

                WireUpLists();
            }
        }

        private void RemoveSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            if (PrizesListBox.SelectedItem != null)
            {
                PrizeModel p = (PrizeModel)PrizesListBox.SelectedItem;

                selectedPrizes.Remove(p);
                

                WireUpLists();
            }
        }

        private void CreateTournamentButton_Click(object sender, EventArgs e)
        {
            // Validate Data
            decimal fee = 0;
            bool feeAcceptable = decimal.TryParse(EnrtyFeeValue.Text, out fee);

            if (!feeAcceptable)
            {
                MessageBox.Show("You need to enter valid entry fee", "Invalid fee", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create out tournament Model
            TournamentModel tm = new TournamentModel();

            tm.TournamentName = TournamentNameValue.Text;
            tm.EntryFee = fee;

            tm.Prizes = selectedPrizes;
            tm.EnteredTaems = selectedTeams;

            // WireUp our Matchups
            TournaamentLogic.CreateRounds(tm);



            // Create Tournament Entry
            // Create all the prizes entries
            // Create all the team entries
            GlobalConfig.Connection.CreateTournament(tm);

            
        }
    }
}
