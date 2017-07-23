using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Security.Credentials;
using Windows.UI.Popups;
using Microsoft.WindowsAzure.MobileServices;

namespace XamarinChallenge.UWP
{
    public sealed partial class MainPage : IAuthenticate
    {
        // Define a member variable for storing the signed-in user. 
        private MobileServiceUser user;
        public MainPage()
        {
            this.InitializeComponent();

            global::XamarinChallenge.App.Init((IAuthenticate)this);

            LoadApplication(new XamarinChallenge.App());
        }

        // Define a method that performs the authentication process
        // using a Facebook sign-in. 
        public async Task<MobileServiceUser> Authenticate()
        {
            string message;
            bool success = false;
            try
            {
                // Change 'MobileService' to the name of your MobileServiceClient instance.
                // Sign-in using Facebook authentication.
                MobileServiceClient mobileClient = new MobileServiceClient(Constants.ApplicationURL);

                user = await mobileClient.LoginAsync(MobileServiceAuthenticationProvider.Facebook);
                message = string.Format("You are now signed in - {0}", user.UserId);
            }
            catch (InvalidOperationException)
            {
                message = "You must log in. Login Required";
            }

            var dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
            return user;
        }
    }
}
