using System;
using CongTraining.Models.Adapters;
using CongTraining.Services;
using System.Collections.Generic;
using CongTraining.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CongTraining.ViewModels {
    public class ContactDetailViewModel : INotifyPropertyChanged {
        private Contact contact;
        private ContactService contactService;
        public event PropertyChangedEventHandler PropertyChanged;

        public ContactDetailViewModel(string name, string address, string phone, int id, string imageSource) {
            contact = new Contact(name, address, phone, id, imageSource);
            contactService = App.Database;
        }

        protected virtual void OnPropertyChanged(string propertyName) {
            var changed = PropertyChanged;
            if (changed != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async Task<ContactAdapter> GetContactById(int givenId) {
            Contact foundContact = await contactService.GetContactById(givenId);
            ContactAdapter temp = new ContactAdapter(foundContact);
            return temp;
        }

        public void UpdateData(ContactAdapter contactAdapter) {
            contactService.UpdateData(contactAdapter.Contact);
        }

        public void SetName(string value) {
            contact.Name = value;
            OnPropertyChanged("Name");
        }

        public void SetAddress(string value) {
            contact.Address = value;
            OnPropertyChanged("Address");
        }

        public void SetPhone(string value) {
            contact.Phone = value;
            OnPropertyChanged("Phone");
        }

        public void SetImageSource(string value) {
            contact.ImageSource = value;
            OnPropertyChanged("ImageSource");
        }

        public string GetName() {
            return contact.Name;
        }

        public string GetAddress() {
            return contact.Address;
        }

        public string GetPhone() {
            return contact.Phone;
        }

        public int GetId() {
            return contact.Id;
        }

        public string GetImageSource() {
            return contact.ImageSource;
        }

    } // class
}
