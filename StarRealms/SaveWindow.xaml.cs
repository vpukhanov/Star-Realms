using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StarRealms
{
    /// <summary>
    /// Логика взаимодействия для SaveWindow.xaml
    /// </summary>
    public partial class SaveWindow : Window
    {
        private static SavesDatabaseEntities savesContext = new SavesDatabaseEntities();
        private SaveLoadMode mode;
        public string SaveString { get; set; }

        public ObservableCollection<SaveFiles> SaveEntries { get; private set; } = new ObservableCollection<SaveFiles>();

        public SaveWindow(SaveLoadMode mode, string save = "")
        {
            InitializeComponent();
            this.DataContext = this;

            this.mode = mode;
            this.SaveString = save;
            this.LoadSaveEntries();

            switch (mode)
            {
                case SaveLoadMode.Load:
                    ChooseOldLabel.Content = "Выберите одно из известных сохранений";
                    CreateNewButton.Content = "или найдите файл на диске";
                    break;

                case SaveLoadMode.Save:
                    ChooseOldLabel.Content = "Перезапишите одно из известных сохранений";
                    CreateNewButton.Content = "или создайте новое";
                    break;
            }
        }

        private void LoadSaveEntries()
        {
            this.InvalidateSavesList();

            foreach (SaveFiles save in savesContext.SaveFiles.OrderByDescending(s => s.LastUpdated))
            {
                this.SaveEntries.Add(save);
            }
        }

        private void InvalidateSavesList()
        {
            foreach (SaveFiles s in savesContext.SaveFiles)
            {
                if (!File.Exists(s.SaveFilePath))
                    savesContext.SaveFiles.Remove(s);
            }

            try
            {
                savesContext.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось обновить информацию в базе данных:\n" + e.Message);
            }
        }

        private void CreateNewButton_Click(object sender, RoutedEventArgs e)
        {
            switch (mode)
            {
                case SaveLoadMode.Load:
                    LoadNewFile();
                    break;

                case SaveLoadMode.Save:
                    SaveNewFile();
                    break;
            }
        }

        private void LoadNewFile()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Сохранение \"Звездные Империи\" (*.srsav)|*.srsav|Файл XML (*.xml)|*.xml";
            openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openDialog.Title = "Загрузить игру";

            if (openDialog.ShowDialog() == true)
                LoadFile(openDialog.FileName);
        }

        private void SaveNewFile()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Сохранение \"Звездные Империи\" (*.srsav)|*.srsav|Файл XML (*.xml)|*.xml";
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveDialog.Title = "Сохранить игру...";

            if (saveDialog.ShowDialog() == true)
                SaveFile(saveDialog.FileName);
        }

        private void LoadFile(string path)
        {
            try
            {
                this.SaveString = File.ReadAllText(path);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Загружаемый файл не найден!");
                return;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Невозможно получить доступ на чтение файла!");
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show("При чтении файла произошла ошибка:\n" + e.Message);
                return;
            }

            SetLastUpdated(path);
            this.DialogResult = true;

            this.Close();
        }

        private void SaveFile(string path)
        {
            try
            {
                File.WriteAllText(path, this.SaveString);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Невозможно получить доступ на чтение файла!");
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show("При чтении файла произошла ошибка:\n" + e.Message);
                return;
            }

            SetLastUpdated(path);
            this.DialogResult = true;

            this.Close();
        }

        private void CloseDialog_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void SetLastUpdated(string path)
        {
            SaveFiles save = savesContext.SaveFiles.Where(s => s.SaveFilePath == path).FirstOrDefault();
            if (save == null)
            {
                save = savesContext.SaveFiles.Create();
                save.SaveFilePath = path;
                savesContext.SaveFiles.Add(save);
            }

            save.LastUpdated = DateTime.Now;

            try
            {
                savesContext.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось обновить информацию в базе данных:\n" + e.Message);
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = savesListView.SelectedIndex;

            if (index >= 0 && index <= SaveEntries.Count)
            {
                string path = SaveEntries[index].SaveFilePath;
                switch (mode)
                {
                    case SaveLoadMode.Load:
                        LoadFile(path);
                        break;

                    case SaveLoadMode.Save:
                        SaveFile(path);
                        break;
                }
            }
        }
    }

    public enum SaveLoadMode
    {
        Save,
        Load
    }
}