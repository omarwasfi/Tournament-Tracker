using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTeamForm : Form
    {

        private List<PersonModel> avalibaleTeamMembers = GlobalConfig.Connection.GetPerson_All();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();
        private ITeamRequester callingForm;

        public CreateTeamForm(ITeamRequester caller)
        {
            InitializeComponent();

            callingForm = caller;

            WireUpLists();
        }

      
        private void CreateSampleData()
        {
            avalibaleTeamMembers.Add(new PersonModel { FirstName = "Tim", LastName = "Corey" });
            avalibaleTeamMembers.Add(new PersonModel { FirstName = "Sue", LastName = "Storm" });

            selectedTeamMembers.Add(new PersonModel { FirstName = "Jane", LastName = "Smith" });
            selectedTeamMembers.Add(new PersonModel { FirstName = "Bill", LastName = "Jones" });

        }

        private void WireUpLists()
        {
            SelectTeamMemberDropDown.DataSource = null;

            SelectTeamMemberDropDown.DataSource = avalibaleTeamMembers;
            SelectTeamMemberDropDown.DisplayMember = "FullName";

            TeamMembersListBox.DataSource = null;

            TeamMembersListBox.DataSource = selectedTeamMembers;
            TeamMembersListBox.DisplayMember = "FullName";

        }

        private void CreateMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PersonModel p = new PersonModel();

                p.FirstName = FirstNameValue.Text;
                p.LastName = LastNameValue.Text;
                p.EmailAddress = EmailValue.Text;
                p.CellPhoneNumber = CellPhoneValue.Text;

                p = GlobalConfig.Connection.CreatePerson(p);

                selectedTeamMembers.Add(p);


                WireUpLists();

                FirstNameValue.Text = "";
                LastNameValue.Text = "";
                EmailValue.Text = "";
                CellPhoneValue.Text = "";

                MessageBox.Show("Member Added");
            }
            else
            {
                MessageBox.Show("you need to fill all your fields");
            }
        }

        private bool ValidateForm()
        {
            if(FirstNameValue.Text.Length == 0)
            {
                return false;
            }
            if (LastNameValue.Text.Length == 0)
            {
                return false;
            }
            if (EmailValue.Text.Length == 0)
            {
                return false;
            }
            if (CellPhoneValue.Text.Length == 0)
            {
                return false;
            }
            return true;
        }

        private void AddMemberButton_Click(object sender, EventArgs e)
        {
            if (SelectTeamMemberDropDown.SelectedItem != null)
            {
                PersonModel p = (PersonModel)SelectTeamMemberDropDown.SelectedItem;

                avalibaleTeamMembers.Remove(p);
                selectedTeamMembers.Add(p);


                WireUpLists();
            }
            
        }

        private void RemoveSelectedMemberButton_Click(object sender, EventArgs e)
        {
            if (TeamMembersListBox.SelectedItem != null) { 
            PersonModel p = (PersonModel)TeamMembersListBox.SelectedItem;

            selectedTeamMembers.Remove(p);
            avalibaleTeamMembers.Add(p);

            WireUpLists();
            }


        }

        private void CreateTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = new TeamModel();

            t.TeamName = TeamNameValue.Text;
            t.TeamMembers = selectedTeamMembers;

            GlobalConfig.Connection.CreateTeam(t);

            callingForm.TeamComplete(t);
            this.Close();

        }
    }
}
