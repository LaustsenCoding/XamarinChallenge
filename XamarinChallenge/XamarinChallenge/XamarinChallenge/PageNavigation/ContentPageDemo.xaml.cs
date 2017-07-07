using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Microsoft.WindowsAzure.MobileServices;

namespace XamarinChallenge.PageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContentPageDemo : ContentPage
    {
        public ContentPageDemo()
        {
            InitializeComponent();
            Title = "Content Page Demo";

            Method(test);
        }

        private static async Task Method(Label test)
        {
            var t = new MobileServiceClient("https://xamarinchallengedemo.azurewebsites.net");
            var items = await t.GetTable<TodoItem>().ReadAsync();
            List<TodoItem> todoItems = items.ToList();
            
            test.Text = items.FirstOrDefault().Text.ToString();
        }
    }
}