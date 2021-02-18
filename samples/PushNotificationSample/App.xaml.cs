using System;
using Plugin.PushNotification;
using Xamarin.Forms;

namespace PushNotificationSample
{
    public partial class App : Application
    {
        readonly MainPage mPage;
        public App()
        {
            InitializeComponent();

            mPage = new MainPage()
            {
                Message = "Hello Push Notifications!"
            };

            MainPage = new NavigationPage(mPage);
        }

        protected override void OnStart()
        {
            SetMessageText($"{mPage.Message}{Environment.NewLine} Current Token: {CrossPushNotification.Current.Token}");

            // Handle when your app starts
            CrossPushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                var message = $"OnTokenRefresh: {p.Token}";
                System.Diagnostics.Debug.WriteLine(message);
                SetMessageText(message);
            };

            CrossPushNotification.Current.OnNotificationError += (s, p) =>
            {
                var message = $"Error: {p.Message}";
                System.Diagnostics.Debug.WriteLine(message);
                SetMessageText(message);
            };

            CrossPushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                    if (p.Data.ContainsKey("body"))
                    {
                        SetMessageText($"{p.Data["body"]}");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            };

            CrossPushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    SetMessageText(p.Identifier);
                }
                else if (p.Data.ContainsKey("color"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        mPage.Navigation.PushAsync(new ContentPage()
                        {
                            BackgroundColor = Color.FromHex($"{p.Data["color"]}")
                        });
                    });
                }
                else if (p.Data.ContainsKey("aps.alert.title"))
                {
                    SetMessageText($"{p.Data["aps.alert.title"]}");
                }
            };
            CrossPushNotification.Current.OnNotificationDeleted += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Dismissed");
            };
        }

        private void SetMessageText(string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                mPage.Message = text;
            });
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
