using System;
using System.ComponentModel;
using SQLite;

namespace CongTraining.Models {
    public class Contact {
        
        private int id;
        private string name;
        private string address;
        private string phone;
        private string imageSource;

        public Contact(string name, string address, string phone, int id, string imageSource) {
            this.name = name;
            this.address = address;
            this.phone = phone;
            this.id = id;
            this.imageSource = imageSource;
        }

        public Contact() {}

        [PrimaryKey, AutoIncrement]
        public int Id { get { return id; } set { id = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string Address { get { return address; } set { address = value; } }
        public string Phone { get { return phone; } set { phone = value; } }
        public string ImageSource { get { return imageSource; } set { imageSource = value; } }
    } // class
}
