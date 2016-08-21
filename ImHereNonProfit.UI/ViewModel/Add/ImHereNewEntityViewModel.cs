using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using ImHereNonProfit.Model;
using ImHereNonProfit.UI.Messages;
using ImHereNonProfit.UI.Utils;

namespace ImHereNonProfit.UI.ViewModel.Add
{
    public class ImHereNewEntityViewModel<T> : ImHereBaseViewModel where T : class 
    {
        protected T NewEntity;
        protected Action<T> OnSuccess;
   
        public ImHereNewEntityViewModel(Action<T> onSuccess)
        {
            Title = $"New {typeof(T).Name}:";
            AppModel = new AppModelContainer();
            OnSuccess = onSuccess;
        }

        protected async void AddNewEntityToSet()
        {
            if (!IsFormValid())
            {
                ImHereMessageBox.ShowError("Validation Errors",$"{Title} Form still has some errors!");
                return;
            }
            try
            {
                AppModel.Set<T>().Add(NewEntity);
                AppModel.SaveChanges();
                AppModel.Entry(NewEntity).State = EntityState.Detached;

                OnSuccess?.Invoke(NewEntity);
                MessengerInstance.Send(new ClosePopUpMessage(null));
            }
            catch (Exception e)
            {
                ImHereMessageBox.ShowError("Error Adding New Entry",e.Message);
            }
        }

        #region Public Commands
        private RelayCommand _addEntityCommand;
        public virtual RelayCommand AddEntityCommand => _addEntityCommand ?? 
            (_addEntityCommand = new RelayCommand(AddNewEntityToSet, IsFormValid));

        #endregion Public Commands
        
        protected virtual bool IsFormValid()
        {
            return true;
        }
    }
}
