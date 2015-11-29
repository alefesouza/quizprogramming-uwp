using Quiz.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Quiz.ViewModels
{
    public class HomeViewModel
    {
        SQLiteConnection objConn;

        public HomeViewModel()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            if (!localSettings.Values.ContainsKey("defaultCategories"))
            {
                CopyDatabase();

                localSettings.Values["defaultCategories"] = true;
            }
            else
            {
                Buscar();
            }
        }

        private async void CopyDatabase()
        {
            StorageFile dbFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Databases/quiz.db"));
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            await dbFile.CopyAsync(localFolder);
            Buscar();
        }

        public void Buscar()
        {
            objConn = new SQLiteConnection("quiz.db");
            string strQ = @"SELECT * FROM categories;";
            var s = objConn.Prepare(strQ);

            while (s.Step() == SQLiteResult.ROW)
            {
                Items.Add(new Home(int.Parse(s[0].ToString()), s[1].ToString()));
            }
        }

        public ObservableCollection<Home> Items { get; private set; } = new ObservableCollection<Home>();
    }
}
