using System;
using Xamarin.Forms;
using System.Collections.Generic;
using CongTraining.ViewModels;
using CongTraining.Models.Adapters;
using System.Collections.ObjectModel;

namespace CongTraining.Views {
    public partial class ContactListPage : ContentPage {

        private ContactListViewModel contactListVM;

        public ContactListPage() {
            InitializeComponent();
            contactListVM = new ContactListViewModel();
            Title = "Contacts";
            this.BindingContext = contactListVM;
        }

        async void OnDelete(object sender, EventArgs e) {
            var mi = ((MenuItem)sender);
            ContactAdapter rowData = (ContactAdapter)mi.CommandParameter;
            var confirmed = await DisplayAlert("Confirm", "Are you sure you want to delete " + rowData.Name + "?", "Yes", "No");
            if (confirmed) {
                contactListVM.DeleteContact(rowData);
                this.OnAppearing();
            }
        }

        async void OnContactSelected(object sender, SelectedItemChangedEventArgs e) {
            ListView listView = (ListView)sender;
            ContactAdapter rowData = (ContactAdapter)listView.SelectedItem;
            await Navigation.PushAsync(new ContactDetailPage(rowData));
        }

        async void OnClickAddContactButton(object sender, EventArgs e) {
            await Navigation.PushAsync(new ContactDetailPage(new ContactAdapter(new Models.Contact("", "", "", 0, ""))));
        }

        protected async override void OnAppearing() {
            base.OnAppearing();
            ObservableCollection<ContactAdapter> temp = await contactListVM.RefreshContacts();
            contactListView.ItemsSource = contactListVM.ContactAdapters;
        }
    } // class
} // namespace