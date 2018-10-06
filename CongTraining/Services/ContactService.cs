using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using CongTraining.Models;

namespace CongTraining.Services {
    public class ContactService {

        private static ContactService instance = null;
        private List<Contact> contacts = new List<Contact>();
        private SQLiteAsyncConnection database;

        public ContactService(string dbPath) {
            Console.WriteLine("DB Path: " + dbPath);
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Contact>().Wait();
        }

        public static ContactService GetInstance(string dbPath) {
            if (instance == null)
                instance = new ContactService(dbPath);
                
            return instance;
        }

        public Task<List<Contact>> GetAllContacts() {
            Task<List<Contact>> temp = database.Table<Contact>().ToListAsync();
            return database.Table<Contact>().OrderBy(t => t.Name).ToListAsync();
        }

        public Task<Contact> GetContactById(int givenId) {
            return database.Table<Contact>().Where(i => i.Id == givenId).FirstOrDefaultAsync();
        }

        public Task<int> DeleteContact(Contact contact) {
            return database.DeleteAsync(contact);
        }

        public Task<int> UpdateData(Contact contact) {
            if (contact.Id != 0)
                return database.UpdateAsync(contact);
            else
                return database.InsertAsync(contact);
        }

    } // class
}
