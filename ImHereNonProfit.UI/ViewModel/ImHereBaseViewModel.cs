using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using ImHereNonProfit.Model;

namespace ImHereNonProfit.UI.ViewModel
{
    public class ImHereBaseViewModel : ViewModelBase
    {
        #region Title
        private String _title;
        public String Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }
        #endregion

        protected Dispatcher AppDispatcher = Application.Current.Dispatcher;
        public bool UserHasExecutivePrivlliges => App.MainLoginService?.LoggedUser?.UserType == UsersTypes.Executive;
        public bool UserHasMemberPrivlliges => App.MainLoginService?.LoggedUser?.UserType >= UsersTypes.Member;

        protected const string EmptyNumbersString = " ----- ";

        protected AppModelContainer AppModel { get; set; }
        
        public override void Cleanup()
        {
            base.Cleanup();
            AppModel?.Dispose();
        }
    }
}
