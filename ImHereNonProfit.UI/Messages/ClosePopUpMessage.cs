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

    public class ClosePopUpMessage : MessageBase
    {
        public Action OnSuccess { get; set; }

        public ClosePopUpMessage(Action onSuccess)
        {
            this.OnSuccess = onSuccess;
        }

    }
}
