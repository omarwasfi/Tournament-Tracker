﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;

namespace TrackerUI
{
    public partial class CreatePrizeForm : Form
    {
        public CreatePrizeForm()
        {
            InitializeComponent();
        }

        private void CreatePrizeButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PrizeModel Model = new PrizeModel(PlaceNameValue.Text, PlaceNumberValue.Text, PrizeAmountValue.Text, PrizePercentageValue.Text);
                foreach (IDataConnection db in GlobalConfig.Connections)
                {
                    db.CreatePrize(Model);
                }
                PlaceNameValue.Text = "";
                PlaceNumberValue.Text = "";
                PrizeAmountValue.Text = "0";
                PrizePercentageValue.Text = "0";
            }
            else
            {
                MessageBox.Show("This form ha invalid indormation , please check this and try again");
            }
        }

        private bool ValidateForm()
        {
            bool Output = true;
            int PlaceNumber = 0;
            if (!int.TryParse(PlaceNumberValue.Text, out PlaceNumber))
            {
                Output = false;
            }
            if (PlaceNumber < 1)
            {
                Output = false;
            }

            if (PlaceNameValue.Text.Length == 0)
            {
                Output = false;
            }

            decimal PrizeAmount = 0;
            double PrizePercentage = 0;

            bool PrizeAmountValid = decimal.TryParse(PrizeAmountValue.Text, out PrizeAmount);
            bool PrizePercentageValid = double.TryParse(PrizePercentageValue.Text, out PrizePercentage);

            if (PrizeAmountValid == false || PrizePercentageValid == false)
            {
                Output = false;
            }
            if(PrizeAmount <= 0 && PrizePercentage <= 0)
            {
                Output = false;
            }

            if(PrizePercentage < 0 || PrizePercentage > 100)
            {
                Output = false;
            }

            return Output;
        }
    }
}
