using System;
using CongTraining.Models.Adapters;
using CongTraining.Services;
using System.Collections.Generic;
using CongTraining.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CongTraining.ViewModels {
    public class ContactListViewModel : INotifyPropertyChanged {
        private ContactService contactService;
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<ContactAdapter> contactAdapters;

        public ContactListViewModel() {
            InitializeListVM();
        }

        private async void InitializeListVM() {
            contactService = App.Database;
            List<Contact> contacts = await contactService.GetAllContacts();
            contactAdapters = new ObservableCollection<ContactAdapter>(ContactAdapter.ToContactAdapters(contacts));
            for (int i = 0; i < contactAdapters.Count; i++) {
                if (contactAdapters[i].ImageSource == "")
                    contactAdapters[i].ImageSource = "usernew.png";
            }
        }

        public ObservableCollection<ContactAdapter> ContactAdapters {
            get {
                return contactAdapters; 
            }
            set { 
                contactAdapters = value; 
                OnPropertyChanged("ContactAdapters"); 
            }
        }

        public async void DeleteContact(ContactAdapter contactAdapter) {
            int temp = await contactService.DeleteContact(contactAdapter.Contact);
        }

        protected virtual void OnPropertyChanged(string propertyName) {
            var changed = PropertyChanged;
            if (changed != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async Task<ObservableCollection<ContactAdapter>> RefreshContacts() {
            List<Contact> contacts = await App.Database.GetAllContacts();
            this.ContactAdapters = new ObservableCollection<ContactAdapter>(ContactAdapter.ToContactAdapters(contacts));
            return contactAdapters;
        }

    } // class
}
