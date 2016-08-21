/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ImHereNonProfit.Model"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System.Data.Entity.Infrastructure;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using ImHereNonProfit.Model;
using Microsoft.Practices.ServiceLocation;

namespace ImHereNonProfit.UI.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            //View Models
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<TopBarViewModel>();
            SimpleIoc.Default.Register<ImHereMainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();

            //Mocks
            SimpleIoc.Default.Register<Mocks>();
        }

        public static MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>(); 
        public static TopBarViewModel TopBar => ServiceLocator.Current.GetInstance<TopBarViewModel>();
        public static ImHereMainViewModel ImHereMain => ServiceLocator.Current.GetInstance<ImHereMainViewModel>();
        public static LoginViewModel Login => ServiceLocator.Current.GetInstance<LoginViewModel>();

        //Mocks
        public static Mocks Mocks => ServiceLocator.Current.GetInstance<Mocks>();
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}