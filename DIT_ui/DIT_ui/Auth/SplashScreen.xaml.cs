using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DIT_ui.Tabs;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat.NavigationPage;

namespace DIT_ui.Auth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreen : ContentPage
    {
        public SplashScreen()
        {
            InitializeComponent();

            AbsoluteLayout.SetLayoutFlags(img_logo,AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(img_logo, new Rectangle(0.5,0.5,AbsoluteLayout.AutoSize,AbsoluteLayout.AutoSize));
        }

        protected void OnStart()
        {
           
        }

        protected override async void OnAppearing()
        {
           
            base.OnAppearing();
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (Application.Current.Properties.ContainsKey("UserMail"))
                {
                    Application.Current.Properties.Clear();
                }

            }
            await img_logo.ScaleTo(1, 2000);
            await img_logo.ScaleTo(0.9, 1500, Easing.Linear);
            await img_logo.ScaleTo(1.5, 500, Easing.Linear);
         
                if (Application.Current.Properties.ContainsKey("UserMail"))
                {
                    await Navigation.PushAsync(new MainPage());
                }
                else
                {
                    await Navigation.PushAsync(new Login());
            }
            
        }
    }
}