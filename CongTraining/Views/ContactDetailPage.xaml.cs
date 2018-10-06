using System;
using System.Collections.Generic;
using CongTraining.ViewModels;
using CongTraining.Models.Adapters;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using Xamarin.Forms;

namespace CongTraining.Views {
    public partial class ContactDetailPage : ContentPage {

        private ContactDetailViewModel contactDetailVM;
        private bool newContact;
        private Geocoder geoCoder;

        public ContactDetailPage(ContactAdapter receivedData) {
            InitializeComponent();
            this.DisplayDetail(receivedData);
        }

        public ContactDetailPage() {
            InitializeComponent();
        }

        public void DisplayDetail(ContactAdapter receivedData) {
            
            geoCoder = new Geocoder();
            int selectedId = receivedData.Id;
            contactDetailVM = new ContactDetailViewModel(receivedData.Name, receivedData.Address,
                                                         receivedData.Phone, receivedData.Id, receivedData.ImageSource);
            this.BindingContext = contactDetailVM;
            this.ChooseDisplayContext(receivedData.Id);
            this.DisplayProfilePicture(profilePicture);
            this.ChangePictureBtnAction(changePictureBtn);
        }

        private async void ChooseDisplayContext(int selectedId) {
            if (selectedId != 0) { // user is modifying existing contact
                newContact = false;
                ContactAdapter contactAdapter = await((ContactDetailViewModel)this.BindingContext).GetContactById(selectedId);
                DisplayInformation(contactAdapter);
            }
            else {
                newContact = true;
                DisplayDefaultDetailPage();
            }
        }

        private void DisplayInformation(ContactAdapter contactAdapter) {
            nameEntry.Text = contactAdapter.Name;
            addressEntry.Text = contactAdapter.Address;
            phoneEntry.Text = contactAdapter.Phone;
            var address = addressEntry.Text;
            this.ChangeMapLocation(address);
        }

        private async void DisplayDefaultDetailPage() {
            nameEntry.Placeholder = "Enter name";
            addressEntry.Placeholder = "Enter address";
            phoneEntry.Placeholder = "Enter phone number";
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            detailMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(100)));
        }

        private void DisplayProfilePicture(Image profilePicture) {
            if (((ContactDetailViewModel)this.BindingContext).GetImageSource() == "")
                profilePicture.Source = "usernew.png";
            else
                profilePicture.Source = ((ContactDetailViewModel)this.BindingContext).GetImageSource();
        }

        private void ChangePictureBtnAction(Button changePictureBtn) {
            changePictureBtn.Clicked += async (sender, args) => {
                if (!CrossMedia.Current.IsPickPhotoSupported) {
                    await DisplayAlert("Photos Not Supported", "Permission not granted to photos.", "OK");
                    return;
                }
                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                });
                if (file == null)
                    return;
                profilePicture.Source = ImageSource.FromStream(() => {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
                ((ContactDetailViewModel)this.BindingContext).SetImageSource(file.Path);
                profilePicture.HeightRequest = 100.0;
            };
        } // ChangePictureBtnAction

        public void OnSubmitButton(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(nameEntry.Text) || String.IsNullOrEmpty(addressEntry.Text) || 
                                                            String.IsNullOrEmpty(phoneEntry.Text)) {
                DisplayAlert("Alert", "Please enter all information", "OK");
                return;
            }
            ContactAdapter updatedInfo = new ContactAdapter(new Models.Contact(nameEntry.Text, 
                                               addressEntry.Text, phoneEntry.Text, 
                                               ((ContactDetailViewModel)this.BindingContext).GetId(), 
                                               ((ContactDetailViewModel)this.BindingContext).GetImageSource()));
            contactDetailVM.UpdateData(updatedInfo);
            if (newContact)
                DisplayAlert("Confirm", "Added " + nameEntry.Text + " successfully", "OK");
            else
                DisplayAlert("Confirm", "Information has been changed", "OK");
            var address = addressEntry.Text;
            this.ChangeMapLocation(address);
        } // OnSubmitButton

        private async void ChangeMapLocation(string address) {
            var approximateLocations = await geoCoder.GetPositionsForAddressAsync(address);
            foreach (var position in approximateLocations) {
                detailMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));
                var pin = new Pin {
                    Type = PinType.Place,
                    Position = position,
                    Label = nameEntry.Text,
                    Address = addressEntry.Text
                };
                detailMap.Pins.Add(pin);
            }
        }

    } // class
} // namespace
