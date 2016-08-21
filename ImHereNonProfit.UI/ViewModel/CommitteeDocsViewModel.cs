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
    public class CommitteeDocsViewModel : ImHereBaseViewModel
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

        #region SelectedDocument
        private CommitteeDocument _selectedDocument;
        public CommitteeDocument SelectedDocument
        {
            get { return _selectedDocument; }
            set { Set(ref _selectedDocument, value); }
        }
        #endregion SelectedDocument

        #region DocumentsList
        private ObservableCollection<CommitteeDocument> _documentsList;
        public ObservableCollection<CommitteeDocument> DocumentsList
        {
            get { return _documentsList; }
            set { Set(ref _documentsList, value); }
        }
        #endregion DocumentsList

        public String DocsTitle => $"{CurrentCommittee.Description()} Documents Archive";
        public String DocsAmount => CurrentCommittee.CommitteeDocuments?.Count.ToString() ?? EmptyNumbersString;
        public String LastUploaded => CurrentCommittee.CommitteeDocuments?.LastOrDefault()?.Name ?? EmptyNumbersString;

        #endregion Public Properties

        #region Public Commands
        private RelayCommand _addDocCommand;
        public RelayCommand AddDocCommand => _addDocCommand ??
                                                    (_addDocCommand = new RelayCommand(DoAddDoc));

        private RelayCommand _removeDocCommand;
        public RelayCommand RemoveDocCommand => _removeDocCommand ??
                                                    (_removeDocCommand = new RelayCommand(DoRemoveDoc, ()=> UserHasExecutivePrivlliges));
        #endregion

        public CommitteeDocsViewModel(Committee committee)
        {
            CurrentCommittee = committee;
            Title = $"{CurrentCommittee.Name} Documents";
            AppModel = new AppModelContainer();

            LoadData();
        }

        private async void LoadData()
        {
            if (AppModel == null) return;
            
            AppModel.Entry(CurrentCommittee).State = EntityState.Unchanged;
            var fetchedDocsFromServer = CurrentCommittee.CommitteeDocuments.ToList();
            DocumentsList = new ObservableCollection<CommitteeDocument>(fetchedDocsFromServer);
        }

        private async void DoRemoveDoc()
        {
            if (SelectedDocument == null)
            {
                MessageBox.Show("Please select a row first");
                return;
            }

            var confirmMessage = $"Are you sure you want to remove {SelectedDocument.Name} from committee documents?";
            if (!ImHereMessageBox.ShowYesNo("Remove Document", confirmMessage)) return;

            var success = true;
            var errorMsg = "";
            try
            {
                AppModel.CommitteeDocuments.Remove(SelectedDocument);
                DocumentsList.Remove(SelectedDocument);
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
        private void DoAddDoc()
        {
            var showAddNewDonationMsg = ShowPopUpMessage.ForEntity<CommitteeDocument>(OnSuccess_AddingNewDoc, CurrentCommittee);
            MessengerInstance.Send(showAddNewDonationMsg);
        }

        private void OnSuccess_AddingNewDoc(object obj)
        {
            AppModel.CommitteeDocuments.Load();
            LoadData();

            RaisePropertyChanged("DocsAmount");
            RaisePropertyChanged("LastUploaded");
        }

    }
}
