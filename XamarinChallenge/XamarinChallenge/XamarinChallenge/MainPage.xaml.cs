using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinChallenge
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void CarouselPageButton_Clicked()
        {
            
        }

        void ContentPageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageNavigation.ContentPageDemo());

        }

        void MasterPageDetailButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageNavigation.MasterDetailPageDemo());
        }

        void NavigationPageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageNavigation.CarouselPageDemo());
        }

        private void TabbedPageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageNavigation.TabbedPageDemo());

        }
    }
}
