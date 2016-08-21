using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using ImHereNonProfit.UI.ViewModel;
using ImHereNonProfit.UI.ViewModel.Add;

namespace ImHereNonProfit.UI.Messages
{

    public class ShowPopUpMessage : MessageBase
    {
        public Window AddNewEntryWindow => (Window)Target;

        public static ShowPopUpMessage ForEntity<T>(Action<object> onSuccess, object extraData = null) where T : class
        {
            var window = ViewsLocator.MatchWindowAndVmToEntity<T>(onSuccess, extraData);
            return new ShowPopUpMessage() {Target = window};
        }
        
    }
}
