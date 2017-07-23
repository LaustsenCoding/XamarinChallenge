using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using XamarinChallenge.Models;
using System.Net;
using System.Net.Http;
using Microsoft.WindowsAzure.Storage.Blob;

namespace XamarinChallenge
{
    public partial class MainPage : ContentPage
    {
        // Track whether the user has authenticated.
        private bool _authenticated = false;
        private const string FBAppID= "691733354348213";
        private BlobStorageToken _blobToken;
        public MainPage()
        {
            InitializeComponent();
        }

        private async Task<Stream> ReadImage()
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync("https://avatars3.githubusercontent.com/u/2000909?v=3&s=200");
            var content = await response.Content.ReadAsStreamAsync();

            return content;
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
                var user = await App.Authenticator.Authenticate();
                if (user != null)
                {
                    _authenticated = true;
                    App.CurrentUser = user;
                }
            }
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            Authenticate();
        }

        private void GetToken_Clicked(object sender, EventArgs e)
        {
            GetToken();
        }

        private async Task<BlobStorageToken> GetToken()
        {
            MobileServiceClient t = new MobileServiceClient(Constants.ApplicationURL);
            t.CurrentUser = App.CurrentUser;
            BlobStorageToken token = null;
            try
            {
                var result = await t.InvokeApiAsync<BlobStorageToken>("/api/GetStorageToken", System.Net.Http.HttpMethod.Get, null);

                token = result;
                _blobToken = token;
            }
            catch (Exception e)
            {
                //TODO TOAST to show error in getting token
                Debug.WriteLine("----------------------------------");
                Debug.WriteLine("Error while attempting to get BlobStorageToken");
                Debug.WriteLine(e.ToString());
                Debug.WriteLine("----------------------------------");
            }

            return token;

        }

        private void PhotoButton_Clicked(object sender, EventArgs e)
        {
            AddNewFileAsync();
        }

        private async Task<Stream> InitCrossMedia()
        {
            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.PickPhotoAsync();
            var stream = file.GetStream();

            return stream;
        }

        public async Task<Stream> GetUploadFileAsync()
        {
            var mediaPlugin = CrossMedia.Current;
            var mainPage = Application.Current.MainPage;

            await mediaPlugin.Initialize();

            if (mediaPlugin.IsPickPhotoSupported)
            {
                var mediaFile = await mediaPlugin.PickPhotoAsync();
                return mediaFile.GetStream();
            }
            else
            {
                await mainPage.DisplayAlert("Media Service Unavailable", "Cannot pick photo", "OK");
                return null;
            }
        }

        private async Task AddNewFileAsync()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            try
            {

                // Get a stream for the file
                //var mediaStream = await ReadImage();
                var mediaStream = await InitCrossMedia();
                if (mediaStream == null)
                {
                    IsBusy = false;
                    return;
                }

                // Get the SAS token from the backend
                var storageToken = await GetToken();

                // Use the SAS token to upload the file
                var storageUri = new Uri($"{storageToken.Uri}{storageToken.SasToken}");
                var blobStorage = new CloudBlockBlob(storageUri);
                await blobStorage.UploadFromStreamAsync(mediaStream);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error Uploading File", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
