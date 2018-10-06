using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace CongTraining.Models.Adapters {
    public class ContactAdapter {
        private Contact contact;

        public ContactAdapter(Contact contact) {
            this.contact = contact;
        }

        public static List<ContactAdapter> ToContactAdapters(List<Contact> contacts) {
            List<ContactAdapter> result = new List<ContactAdapter>();
            for (int i = 0; i < contacts.Count; i++) {
                if (contacts[i].ImageSource == "")
                    contacts[i].ImageSource = "usernew.png";
                result.Add(new ContactAdapter(contacts[i]));
            }
            return result;
        } 

        public Contact Contact { get { return contact; } }
        public string Name { get { return contact.Name; } set { contact.Name = value;} }
        public string Address { get { return contact.Address; } set { contact.Address = value; } }
        public string Phone { get { return contact.Phone; } set { contact.Phone = value; } }
        public int Id { get { return contact.Id; } set { contact.Id = value; } }
        public string ImageSource { get { return contact.ImageSource; } set { contact.ImageSource = value; } }
    }
}
