using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImHereNonProfit.UI.Utils
{
    public class ImHereMessageBox
    {

        public static MessageBoxResult Show(string title, string message, MessageBoxImage icon = MessageBoxImage.None, MessageBoxButton button = MessageBoxButton.OK)
        {
            return MessageBox.Show(Application.Current.MainWindow, message, title, button, icon);
        }

        public static void ShowError(string title, string message)
        {
            Show(title,message,MessageBoxImage.Error);
        }

        public static bool ShowYesNo(string title, string message)
        {
            var result = Show(title, message, MessageBoxImage.Question, MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes ? true : false;
        }

    }
}
