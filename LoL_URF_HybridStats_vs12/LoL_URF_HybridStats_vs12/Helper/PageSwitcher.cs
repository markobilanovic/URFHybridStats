using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using LoL_URF_HybridStats_vs12.UI_Controls;

namespace LoL_URF_HybridStats_vs12.Helper
{
    public class PageSwitcher
    {
        public static MainWindow pageSwitcher;

        public static void Switch(UserControl newPage)
        {
            pageSwitcher.Navigate(newPage);
        }


        public static void SwitchToMainPage()
        {
            pageSwitcher.Navigate(MainWindowPage.Instance);
            FocusManager.SetFocusedElement(pageSwitcher, MainWindowPage.Instance);
            Keyboard.Focus(MainWindowPage.Instance);
        }
    }
}
