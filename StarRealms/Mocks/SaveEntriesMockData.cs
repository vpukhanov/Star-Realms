using System;
using System.Collections.ObjectModel;

namespace StarRealms.Mocks
{
    internal class SaveEntriesMockData
    {
        public ObservableCollection<SaveFiles> SaveEntries = new ObservableCollection<SaveFiles>();

        public SaveEntriesMockData()
        {
            SaveEntries.Add(new SaveFiles() { Id = 1, LastUpdated = DateTime.MinValue, SaveFilePath = @"C:\Temp\File1.srsav" });
            SaveEntries.Add(new SaveFiles() { Id = 2, LastUpdated = DateTime.MinValue, SaveFilePath = @"C:\Temp\File2.srsav" });
            SaveEntries.Add(new SaveFiles() { Id = 3, LastUpdated = DateTime.MinValue, SaveFilePath = @"C:\Temp\File3.srsav" });
            SaveEntries.Add(new SaveFiles() { Id = 4, LastUpdated = DateTime.MinValue, SaveFilePath = @"C:\Temp\File4.srsav" });
            SaveEntries.Add(new SaveFiles() { Id = 5, LastUpdated = DateTime.MinValue, SaveFilePath = @"C:\Temp\File5.srsav" });
            SaveEntries.Add(new SaveFiles() { Id = 6, LastUpdated = DateTime.MinValue, SaveFilePath = @"C:\Temp\File6.srsav" });
        }
    }
}