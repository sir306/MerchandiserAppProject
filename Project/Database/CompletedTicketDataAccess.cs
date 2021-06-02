using Project.DataClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Project.Database
{
    class CompletedTicketDataAccess : INotifyPropertyChanged
    {
        private ObservableCollection<CompletedTickets> _completedTickets;
        public ObservableCollection<CompletedTickets> CompletedTickets { get { return _completedTickets; } set { _completedTickets = value;
                OnPropertyChanged(nameof(CompletedTickets)); } }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private static readonly object collisionLock = new object();
        private readonly SQLiteConnection database;

        public CompletedTicketDataAccess()
        {
            if (database == null)
            {
                database = DependencyService.Get<IDatabaseConnection>().DbConnectionCompleted();
                database.CreateTable<CompletedTickets>();
                this.CompletedTickets = new ObservableCollection<CompletedTickets>(database.Table<CompletedTickets>());
            }
            if (database != null) { SortCollection(); }
        }
        //sort
        private void SortCollection()
        {
            if (CompletedTickets.Count != 0)
            {
                CompletedTickets = new ObservableCollection<CompletedTickets>(CompletedTickets.OrderBy(x => x.OrderRef.Date));
            }
        }
        //add ticket method
        public void AddNewTicket(CompletedTickets item)
        {
            this.CompletedTickets.Add(item);
        }

        //retrieve ticket method
        public CompletedTickets GetTicket(int id)
        {
            lock (collisionLock)
            {
                return database.Table<CompletedTickets>().FirstOrDefault(Tickets => Tickets.Id == id);
            }
        }

        //save ticket
        public int SaveTicket(CompletedTickets ticketsInstance)
        {
            lock (collisionLock)
            {
                if (ticketsInstance.Id != 0)
                {
                    database.Update(ticketsInstance);
                }
                else
                {
                    database.Insert(ticketsInstance);
                }
                database.Commit();
                return ticketsInstance.Id;
            }
        }
        public void SaveAllTickets()
        {
            lock (collisionLock)
            {
                foreach (var ticketsInstance in this.CompletedTickets)
                {
                    if (ticketsInstance.Id != 0)
                    {
                        database.Update(ticketsInstance);
                    }
                    else
                    {
                        database.Insert(ticketsInstance);
                    }
                }
            }
        }

        //delete Ticket
        public int DeleteTicket(CompletedTickets ticketsInstance)
        {
            var id = ticketsInstance.Id;
            if (id != 0)
            {
                lock (collisionLock)
                {
                    database.Delete<CompletedTickets>(id);
                }
            }
            this.CompletedTickets.Remove(ticketsInstance);
            return id;
        }

        //delete all tickets
        public void DeleteAllTickets()
        {
            lock (collisionLock)
            {
                database.DropTable<CompletedTickets>();
                database.CreateTable<CompletedTickets>();
            }
            this.CompletedTickets = null;
            this.CompletedTickets = new ObservableCollection<CompletedTickets>(database.Table<CompletedTickets>());
        }
    }
}

