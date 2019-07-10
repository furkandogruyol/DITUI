using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace DIT_ui.Tabs
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CartDone : ContentPage
	{
	    private ZXingBarcodeImageView barcode;

        public CartDone ()
		{
            InitializeComponent();
		    barcode = new ZXingBarcodeImageView
		    {
		        HorizontalOptions = LayoutOptions.FillAndExpand,
		        VerticalOptions = LayoutOptions.FillAndExpand,
		        AutomationId = "zxingBarcodeImageView"
		    };
		    barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
		    barcode.BarcodeOptions.Width = 300;
		    barcode.BarcodeOptions.Height = 300;
		    barcode.BarcodeOptions.Margin = 10;
		    barcode.BarcodeValue = Application.Current.Properties["CartId"].ToString();

		   stack_barcode.Children.Insert(1,barcode);
        }
        
	}
}