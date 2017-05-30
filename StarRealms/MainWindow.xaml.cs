using StarRealms;
using StarRealms.RulesEngine;
using StarRealms.RulesEngine.Abstract;
using StarRealms.RulesEngine.Cards;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StarRealms_Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game CurrentGame;

        public MainWindow()
        {
            InitializeComponent();

            this.CurrentGame = this.Resources["CurrentGame"] as Game;
            this.CurrentGame.OnGameOver += CurrentGame_OnGameOver;

            Utilities.userNotifyMethod = ShowNotification;
            Utilities.userCardChoiceMethod = ChooseOneCard;
            Utilities.userYesNoChoiceMethod = ChooseYesNo;
            Utilities.userStringChoiceMethod = ChooseString;
        }

        public void ShowNotification(string notification)
        {
            MessageBox.Show(notification, "Сообщение");
        }

        public bool ChooseYesNo(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        public Card ChooseOneCard(List<Card> cards)
        {
            CardChooser cc = new CardChooser();
            cc.SetCards(cards);
            cc.ShowDialog();

            if (cc.DialogResult == System.Windows.Forms.DialogResult.OK && cc.SelectedIndex >= 0)
            {
                return cards[cc.SelectedIndex];
            }
            else
            {
                return null;
            }
        }

        public int ChooseString(List<string> choices)
        {
            StringChooser sc = new StringChooser();
            sc.SetChoices(choices);
            sc.ShowDialog();

            if (sc.DialogResult == System.Windows.Forms.DialogResult.OK && sc.SelectedIndex >= 0)
            {
                return sc.SelectedIndex;
            }
            else
            {
                return -1;
            }
        }

        private void CurrentGame_OnGameOver(Player player)
        {
            if (player == this.CurrentGame.Human)
            {
                MessageBox.Show("Вы проиграли!");
            }
            else
            {
                MessageBox.Show("Вы победили!");
            }

            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            this.Close();
        }

        private void PlayerHand_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.CurrentGame.IsHumanTurn)
                return;

            Card clickedCard = (sender as Image).Tag as Card;
            this.CurrentGame.CurrentPlayer.PlayCard(this.CurrentGame, clickedCard);
        }

        private void EndTurn_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentGame.NextTurn();
        }

        private void TradeExplorer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.CurrentGame.IsHumanTurn)
                return;

            this.CurrentGame.CurrentPlayer.PurchaseCard(new Explorer());
        }

        private void BotAvatar_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.CurrentGame.IsHumanTurn)
                return;

            this.CurrentGame.Bot.RemoveAuthority(this.CurrentGame.CurrentPlayer.AvailableDamage);
            this.CurrentGame.CurrentPlayer.RemoveDamage(this.CurrentGame.CurrentPlayer.AvailableDamage);
        }

        private void TradeRow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.CurrentGame.IsHumanTurn)
                return;

            Card clickedCard = (sender as Image).Tag as Card;
            this.CurrentGame.TradeRow.PurchaseCard(this.CurrentGame.CurrentPlayer, clickedCard);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: заменить на отдельное окно
            string saveString = "";
            try
            {
                saveString = this.CurrentGame.SaveGame();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при сохранении состояния игры:\n" + ex.Message);
                return;
            }

            SaveWindow saveWindow = new SaveWindow(SaveLoadMode.Save, saveString);
            if (saveWindow.ShowDialog() == true)
            {
                MessageBox.Show("Игра сохранена!");
            }
            /*SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Сохранение \"Звездные Империи\" (*.srsav)|*.srsav|Файл XML (*.xml)|*.xml";
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveDialog.Title = "Сохранить игру...";

            if (saveDialog.ShowDialog() == true)
            {
                string save = this.CurrentGame.SaveGame();
                File.WriteAllText(saveDialog.FileName, save);
            }*/
        }

        public void LoadGame(string save)
        {
            try
            {
                this.CurrentGame.LoadGame(save);
            }
            catch (Exception e)
            {
                MessageBox.Show("Произошла ошибка при загрузке состояния игры:\n" + e.Message);
            }
        }
    }
}