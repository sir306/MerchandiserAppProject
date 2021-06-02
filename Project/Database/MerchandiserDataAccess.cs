using Project.DataClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Project.Database
{
    class MerchandiserDataAccess : INotifyPropertyChanged
    {
        private ObservableCollection<Merchandiser> _merchandisers;
        public ObservableCollection<Merchandiser> Merchandisers { get { return _merchandisers; } set { _merchandisers = value; OnPropertyChanged(nameof(Merchandisers)); } }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private static readonly object collisionLock = new object();
        private readonly SQLiteConnection database;

        public MerchandiserDataAccess()
        {
            if (database == null)
            {
                database = DependencyService.Get<IDatabaseConnection>().DbConnectionMerchList();
                database.CreateTable<Merchandiser>();
                this.Merchandisers = new ObservableCollection<Merchandiser>(database.Table<Merchandiser>());
            }
        }
        //add Merchandiser method
        public void AddNewMerch(Merchandiser item)
        {
            this.Merchandisers.Add(item);
        }

        //retrieve Merchandiser method
        public Merchandiser GetMerch(int id)
        {
            lock (collisionLock)
            {
                return database.Table<Merchandiser>().FirstOrDefault(Merchandisers => Merchandisers.MerchId == id);
            }
        }

        //save Merchandiser
        public int SaveMerch(Merchandiser MerchandiserInstance)
        {
            lock (collisionLock)
            {
                if (MerchandiserInstance.MerchId != 0)
                {
                    database.Update(MerchandiserInstance);
                }
                else
                {
                    database.Insert(MerchandiserInstance);
                }
                database.Commit();
                return MerchandiserInstance.MerchId;
            }
        }
        public void SaveAllMerchandisers()
        {
            lock (collisionLock)
            {
                foreach (var MerchandiserInstance in this.Merchandisers)
                {
                    if (MerchandiserInstance.MerchId != 0)
                    {
                        database.Update(MerchandiserInstance);
                    }
                    else
                    {
                        database.Insert(MerchandiserInstance);
                    }
                }
            }
        }

        //delete Merchandiser
        public int DeleteMerch(Merchandiser MerchandiserInstance)
        {
            var id = MerchandiserInstance.MerchId;
            if (id != 0)
            {
                lock (collisionLock)
                {
                    database.Delete<Merchandiser>(id);
                }
            }
            this.Merchandisers.Remove(MerchandiserInstance);
            return id;
        }

        //delete all Merchandiser
        public void DeleteAllMerchandisers()
        {
            lock (collisionLock)
            {
                database.DropTable<Merchandiser>();
                database.CreateTable<Merchandiser>();
            }
            this.Merchandisers = null;
            this.Merchandisers = new ObservableCollection<Merchandiser>(database.Table<Merchandiser>());
        }
    }
}

