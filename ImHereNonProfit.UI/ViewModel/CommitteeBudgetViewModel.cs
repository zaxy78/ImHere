using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ImHereNonProfit.Model;
using ImHereNonProfit.UI.Messages;
using ImHereNonProfit.UI.Utils;

namespace ImHereNonProfit.UI.ViewModel
{
    public class CommitteeBudgetViewModel : ImHereBaseViewModel
    {

        #region Public Properties

        #region CurrentCommittee
        private Committee _currentCommittee;
        public Committee CurrentCommittee
        {
            get { return _currentCommittee; }
            set { Set(ref _currentCommittee, value); }
        }
        #endregion CurrentCommittee

        #region SelectedExpens
        private CommitteeExpens _selectedExpens;
        public CommitteeExpens SelectedExpens
        {
            get { return _selectedExpens; }
            set { Set(ref _selectedExpens, value); }
        }
        #endregion SelectedExpens

        #region ExpenssesList
        private ObservableCollection<CommitteeExpens> _expenssesList;
        public ObservableCollection<CommitteeExpens> ExpenssesList
        {
            get { return _expenssesList; }
            set { Set(ref _expenssesList, value); }
        }
        #endregion ExpenssesList  

        public bool HasBudget => CurrentCommittee.CurrentYearBudget.HasValue;

        public String BudgetAmount => CurrentCommittee.CurrentYearBudget?.ToString() ?? EmptyNumbersString;

        public String TotalExpensess => GetCommitteeTotalExpensess()?.ToString() ?? EmptyNumbersString;

        public String LeftResources => GetCommitteeLeftResources()?.ToString() ?? EmptyNumbersString;

        public String LastExpens => CurrentCommittee.CommitteeExpenses?.LastOrDefault()?.Amount.ToString() ?? EmptyNumbersString;

        public String BudgetTitle
        {
            get
            {
                if (HasBudget)
                {
                    return $"The Budget for Year {DateTime.Now.Year} is: ";
                }
                return $"No Budget was set was {CurrentCommittee.Description()}, " +
                        $"Please contact the Chair {CurrentCommittee.Chair.FullName()}";
            }
        }

        #endregion

        #region Public Commands
        private RelayCommand _addExpenssCommand;
        public RelayCommand AddExpenssCommand => _addExpenssCommand ?? 
                                                    (_addExpenssCommand = new RelayCommand(DoAddExpenss));    
        
        private RelayCommand _removeExpenssCommand;
        public RelayCommand RemoveExpenssCommand => _removeExpenssCommand ?? 
                                                    (_removeExpenssCommand = new RelayCommand(DoRemoveExpenss, () => UserHasExecutivePrivlliges));

        #endregion

        public CommitteeBudgetViewModel(Committee committee)
        {
            CurrentCommittee = committee;
            Title = $"{CurrentCommittee.Name} Budget & Expensses";
            AppModel = new AppModelContainer();

            LoadData();
        }

        #region Private Methods
        private void LoadData()
        {
            AppModel.Entry(CurrentCommittee).State = EntityState.Unchanged;
            var fetchedExpenssesFromServer = CurrentCommittee.CommitteeExpenses.ToList();
            ExpenssesList = new ObservableCollection<CommitteeExpens>(CurrentCommittee.CommitteeExpenses);
        }

        private int? GetCommitteeTotalExpensess()
        {
            int? totalExpensses = null;
            AppModel.Entry(CurrentCommittee).State = EntityState.Unchanged;
            totalExpensses = CurrentCommittee?.CommitteeExpenses?.Sum(exp => exp.Amount);
            return totalExpensses;
        }

        private int? GetCommitteeLeftResources()
        {
            int? leftResources = CurrentCommittee.CurrentYearBudget - GetCommitteeTotalExpensess();
            return leftResources;
        }

        private async void DoRemoveExpenss()
        {
            if (SelectedExpens == null)
            {
                MessageBox.Show("Please select a row first");
                return;
            }

            var confirmMessage = $"Are you sure you want to remove this expens from committee expennsees?";
            if (!ImHereMessageBox.ShowYesNo("Remove Expenss", confirmMessage)) return;

            var success = true;
            var errorMsg = "";
            try
            {
                AppModel.CommitteeExpenses.Remove(SelectedExpens);
                ExpenssesList.Remove(SelectedExpens);
                await AppModel.SaveChangesAsync();
            }
            catch (Exception e)
            {
                success = false;
                errorMsg = e.Message;
            }

            if (success) return;
            ImHereMessageBox.ShowError("Error deleting row!", errorMsg);
        }
        private void DoAddExpenss()
        {
            var showAddNewDonationMsg = ShowPopUpMessage.ForEntity<CommitteeExpens>(OnSuccess_AddingNewExpenss, CurrentCommittee);
            MessengerInstance.Send(showAddNewDonationMsg);
        }

        private void OnSuccess_AddingNewExpenss(object obj)
        {
            AppModel.CommitteeExpenses.Load();
            LoadData();

            RaisePropertyChanged("TotalExpensess");   
            RaisePropertyChanged("LeftResources");
            RaisePropertyChanged("LastExpens");
        }


        #endregion Private Methods

    }
}
