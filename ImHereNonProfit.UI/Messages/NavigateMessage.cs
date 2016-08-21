using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using ImHereNonProfit.UI.ViewModel;

namespace ImHereNonProfit.UI.Messages
{

    public class NavigateMessage : MessageBase
    {
        public NavigateMessage(Page toPage) : base(null, toPage) { }
        public NavigateMessage(object sender, Page toPage) : base(sender, toPage) { }
        public NavigateMessage(ImHereBaseViewModel toViewModel) : 
            base(null, ViewsLocator.MatchViewToViewModel(toViewModel)) { }
   
        public Page ToPage => (Page) Target;
    }
}
