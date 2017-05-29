using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StarRealms
{
    public partial class StringChooser : Form
    {
        public int SelectedIndex { get; set; }

        public StringChooser()
        {
            InitializeComponent();
        }

        public void SetChoices(List<string> choices)
        {
            comboBoxChoices.Items.AddRange(choices.ToArray());
            comboBoxChoices.SelectedIndex = 0;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.SelectedIndex = comboBoxChoices.SelectedIndex;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}