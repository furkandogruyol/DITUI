using System;
using DIT_ui.Auth;
using DIT_ui.Tabs;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DIT_ui
{
    public partial class App : Application
    {
        public App()
        {
            LiveReload.Init();
            InitializeComponent();
            
            MainPage = new NavigationPage(new SplashScreen())
            {
               
                BarBackgroundColor = Color.FromHex("#082631"),
                BarTextColor = Color.FromHex("#FFFFFF")

            };
        }

        protected  override void OnStart()
        {
           
        }

        protected override void OnSleep()
        {
           if (Application.Current.Properties.ContainsKey("CartId"))
           {
               Application.Current.Properties.Remove("CartId");
           }

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
