using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinChallenge.PageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContentPageDemo : ContentPage
    {
        public ContentPageDemo()
        {
            InitializeComponent();
            Title = "Content Page Demo";
        }
    }
}