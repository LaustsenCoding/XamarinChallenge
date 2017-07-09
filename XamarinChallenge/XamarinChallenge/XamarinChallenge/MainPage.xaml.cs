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
                var t = new MobileServiceClient("https://xamarinchallengedemo.azurewebsites.net");
                TodoItem item = new TodoItem { Text = "Awesome item" };
                t.GetTable<TodoItem>().InsertAsync(item);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
    }
}
