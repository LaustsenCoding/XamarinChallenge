using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using XamarinChallenge.Models;

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

        private async void LoadImgFromBlob()
        {
            Stream stream = new MemoryStream();
            try
            {
                stream = await DownloadImage();
                Img.Source = ImageSource.FromStream(() => new MemoryStream(stream.ReadByte()));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public async Task<MemoryStream> DownloadImage()
        {
            string storageAccountName = "xamarinchallenge";
            MemoryStream stream = new MemoryStream();

            try
            {
                var token = await GetToken();
                //var ttt = token.SasToken.Remove(0, 1);
                var blobStorage = new CloudBlockBlob(new Uri($"{token.Uri}{token.SasToken}"));
                var blobContainer = blobStorage.Container.GetBlobReference("533445f0e34a4991b2add7a343533810");

                int i = 1;

                await blobContainer.DownloadToStreamAsync(stream);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            return stream;
        }

        private async Task<BlobStorageToken> GetToken()
        {
            if (App.CurrentUser == null)
            {

                return null;
            }
            MobileServiceClient t = new MobileServiceClient(Constants.ApplicationURL);
            t.CurrentUser = App.CurrentUser;

            BlobStorageToken token = null;
            try
            {
                var result = await t.InvokeApiAsync<BlobStorageToken>("/api/GetStorageToken", System.Net.Http.HttpMethod.Get, null);

                token = result;
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

        private void LoadImage_Clicked(object sender, EventArgs e)
        {
            LoadImgFromBlob();
        }
    }
}