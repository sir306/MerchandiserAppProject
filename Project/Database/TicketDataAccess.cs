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
    class TicketDataAccess : INotifyPropertyChanged
    {
        private readonly SQLiteConnection database;
        private static object collisionLock = new object();
        private ObservableCollection<Ticket> _tickets;
        public ObservableCollection<Ticket> Tickets { get { return _tickets; } set { _tickets = value; OnPropertyChanged(nameof(Tickets)); } }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TicketDataAccess()
        {
            if(database == null)
            {
                database = DependencyService.Get<IDatabaseConnection>().DbConnection();
                database.CreateTable<Ticket>();
                this.Tickets = new ObservableCollection<Ticket>(database.Table<Ticket>());
            }
            if (database != null) { SortCollection(); }
        }
        private void SortCollection()
        {
            if(Tickets.Count != 0)
            {
                Tickets = new ObservableCollection<Ticket>(Tickets.OrderBy(x => x.OrderRef.Date));
            }
        }
        //add ticket method
        public void AddNewTicket(Ticket item)
        {
            this.Tickets.Add(item);
            SortCollection();
        }

        //retrieve ticket method
        public Ticket GetTicket(int id)
        {
            lock (collisionLock)
            {
                return database.Table<Ticket>().FirstOrDefault(Tickets => Tickets.Id == id);
            }
        }

        //save ticket
        public int SaveTicket(Ticket ticketsInstance)
        {
            lock (collisionLock)
            {
                if (ticketsInstance.Id != 0)
                {
                    database.Update(ticketsInstance);
                    SortCollection();
                    return ticketsInstance.Id;
                }
                else
                {
                    database.Insert(ticketsInstance);
                    SortCollection();
                    return ticketsInstance.Id;
                }                
            }
        }
        public void SaveAllTickets()
        {
            lock (collisionLock)
            {
                foreach (var ticketsInstance in this.Tickets)
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
                SortCollection();
            }
        }

        //delete Ticket
        public int DeleteTicket(Ticket ticketsInstance)
        {
            var id = ticketsInstance.Id;
            if (id != 0)
            {
                lock (collisionLock)
                {
                    database.Delete<Ticket>(id);
                    OnPropertyChanged(nameof(Tickets));
                }
            }
            this.Tickets.Remove(ticketsInstance);
            OnPropertyChanged(nameof(Tickets));
            return id;
        }

        //delete all tickets
        public void DeleteAllTickets()
        {
            lock (collisionLock)
            {
                database.DropTable<Ticket>();
                database.CreateTable<Ticket>();
            }
            this.Tickets = null;
            this.Tickets = new ObservableCollection<Ticket>(database.Table<Ticket>());
            OnPropertyChanged(nameof(Tickets));
        }
    }
}
