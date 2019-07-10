using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIT_ui.Customs;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Xamarin.Forms;
using DIT_ui.iOS;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer))]
namespace DIT_ui.iOS
{
    public class CustomViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            var view = item as CustomViewCell;
            cell.SelectedBackgroundView = new UIView
            {
                BackgroundColor = view.SelectedItemBackgroundColor.ToUIColor(),
            };

            return cell;
        }
    }
}