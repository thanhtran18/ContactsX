using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CongTraining.Views;
using CongTraining.Services;
using CongTraining.Models;
using System;
using CongTraining.ViewModels;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CongTraining {
    public partial class App: Application {
        
        public static IList<string> Contacts { get ; set; }
        static ContactService database;

        public App() {
            InitializeComponent();
            MainPage = new NavigationPage(new ContactListPage()){ BarBackgroundColor = Color.FromRgb(52, 159, 216) };
        }

        public static ContactService Database {
            get {
                if (database == null)
                    database = new ContactService(DependencyService.Get<IFileHelper>().GetLocalFilePath("ContactDatabase.db"));
                return database;
            }

        }

        protected override void OnStart() {
            // Handle when your app starts
        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }
    } // class
} // namespace