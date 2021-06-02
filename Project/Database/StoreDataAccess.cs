using Project.DataClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Project.Database
{
    class StoreDataAccess : INotifyPropertyChanged
    {
        private readonly SQLiteConnection database;
        private static object collisionLock = new object();
        private ObservableCollection<StoreDetails> _storeDetails;
        public ObservableCollection<StoreDetails> StoreDetails { get { return _storeDetails; } set { 
                _storeDetails = value; OnPropertyChanged(nameof(StoreDetails)); } }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StoreDataAccess()
        {
            if(database == null)
            {
                database = DependencyService.Get<IDatabaseConnection>().DbConnectionStore();
                database.CreateTable<StoreDetails>();
                this.StoreDetails = new ObservableCollection<StoreDetails>(database.Table<StoreDetails>());
            }
        }
        //add ticket method
        public void AddNewStore(StoreDetails item)
        {
            this.StoreDetails.Add(item);
        }

        //retrieve ticket method
        public StoreDetails GetStoreDetails(int id)
        {
            lock (collisionLock)
            {
                return database.Table<StoreDetails>().FirstOrDefault(StoreDetails => StoreDetails.StoreId == id);
            }
        }

        //save ticket
        public int SaveStoreDetails(StoreDetails storeDetailsInstance)
        {
            lock (collisionLock)
            {
                if (storeDetailsInstance.StoreId != 0)
                {
                    database.Update(storeDetailsInstance);
                    return storeDetailsInstance.StoreId;
                }
                else
                {
                    database.Insert(storeDetailsInstance);
                    return storeDetailsInstance.StoreId;
                }
                //database.Commit();
            }
        }
        public void SaveAllStoreDetails()
        {
            lock (collisionLock)
            {
                foreach (var storeDetailsInstance in this.StoreDetails)
                {
                    if (storeDetailsInstance.StoreId != 0)
                    {
                        database.Update(storeDetailsInstance);
                    }
                    else
                    {
                        database.Insert(storeDetailsInstance);
                    }
                }
            }
        }

        //delete Ticket
        public int DeleteStoreDetails(StoreDetails storeDetailsInstance)
        {
            var id = storeDetailsInstance.StoreId;
            if (id != 0)
            {
                lock (collisionLock)
                {
                    database.Delete<StoreDetails>(id);
                }
            }
            this.StoreDetails.Remove(storeDetailsInstance);
            return id;
        }

        //delete all tickets
        public void DeleteAllStoreDetails()
        {
            lock (collisionLock)
            {
                database.DropTable<StoreDetails>();
                database.CreateTable<StoreDetails>();
            }
            this.StoreDetails = null;
            this.StoreDetails = new ObservableCollection<StoreDetails>(database.Table<StoreDetails>());
        }
    }
}
