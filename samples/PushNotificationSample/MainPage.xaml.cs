using Plugin.PushNotification;
using Xamarin.Forms;

namespace PushNotificationSample
{
    public partial class MainPage : ContentPage
    {

        public string Message
        {
            get
            {
                return textLabel.Text;
            }
            set
            {
                textLabel.Text = value;
            }
        }

        public MainPage()
        {
            InitializeComponent();
        }

        void Reset_Clicked(object sender, System.EventArgs e)
        {
            Message = string.Empty;
            CrossPushNotification.Current.UnregisterForPushNotifications();
            CrossPushNotification.Current.RegisterForPushNotifications();
        }
    }
}
