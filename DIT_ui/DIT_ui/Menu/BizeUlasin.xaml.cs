using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIT_ui.Menu
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BizeUlasin : ContentPage
	{
		public BizeUlasin ()
		{
			InitializeComponent ();
		}

	    private void Btn_mail_OnClicked(object sender, EventArgs e)
	    {
            //İOS ta calısmıyor olabilir kontrol et.
	        string strMailTo = @"mailto:tfapphub@gmail.com?Subject:test&Body:testBody";
	        Device.OpenUri(new Uri(strMailTo));
        }
    }
}