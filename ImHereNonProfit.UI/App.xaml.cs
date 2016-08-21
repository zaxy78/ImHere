using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using ImHereNonProfit.UI.Services;
using ImHereNonProfit.UI.ViewModel;

namespace ImHereNonProfit.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static NavigationService MainNavigationService;
        public static LoginService MainLoginService = new LoginService();

        public static void MainWindow_OnLoaded()
        {
            ViewModelLocator.Main.Init_OnLoad();
            ViewModelLocator.TopBar.Init_OnLoad();
        }
    }
}
