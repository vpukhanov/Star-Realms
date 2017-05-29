using StarRealms.RulesEngine.Abstract;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StarRealms
{
    public partial class CardChooser : Form
    {
        public int SelectedIndex { get; set; }

        private List<Card> cards;

        public CardChooser()
        {
            InitializeComponent();
        }

        public void SetCards(List<Card> cards)
        {
            this.cards = cards;
            cardsComboBox.Items.AddRange(cards.ToArray());
            cardsComboBox.SelectedIndex = 0;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.SelectedIndex = cardsComboBox.SelectedIndex;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cardsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            descriptionLabel.Text = this.cards[cardsComboBox.SelectedIndex].Description;
        }
    }
}