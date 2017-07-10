using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
using System.Diagnostics;

namespace XamarinChallenge
{
    public partial class MainPage : ContentPage
    {
        // Track whether the user has authenticated.
        private bool authenticated = false;
        private const string FBAppID= "691733354348213";
        public MainPage()
        {
            InitializeComponent();


        }

        void CarouselPageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageNavigation.CarouselPageDemo());
        }

        void ContentPageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageNavigation.ContentPageDemo());
        }

        void MasterPageDetailButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageNavigation.MasterDetailPageDemo());
        }

        private void TabbedPageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageNavigation.TabbedPageDemo());

        }

        void storage()
        {
            try
            {
                var t = new MobileServiceClient(Constants.ApplicationURL);
                TodoItem item = new TodoItem { Text = "Awesome item" };
                t.GetTable<TodoItem>().InsertAsync(item);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        private async void Authenticate()
        {
            // Refresh items only when authenticated.
            if (App.Authenticator != null)
            {
                authenticated = await App.Authenticator.Authenticate();
            }
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            Authenticate();
        }
    }
}
