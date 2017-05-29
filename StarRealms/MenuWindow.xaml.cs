using StarRealms_Client;
using System.Windows;

namespace StarRealms
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            // TODO: заменить на отдельное окно
            SaveWindow saveWindow = new SaveWindow(SaveLoadMode.Load);
            if (saveWindow.ShowDialog() == true)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.LoadGame(saveWindow.SaveString);
                mainWindow.Show();
                this.Close();
            }

            /*OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Сохранение \"Звездные Империи\" (*.srsav)|*.srsav|Файл XML (*.xml)|*.xml";
            openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openDialog.Title = "Загрузить игру";

            if (openDialog.ShowDialog() == true)
            {
                string save = File.ReadAllText(openDialog.FileName);

                MainWindow mainWindow = new MainWindow();
                mainWindow.LoadGame(save);
                mainWindow.Show();
                this.Close();
            }*/
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}