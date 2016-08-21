using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImHereNonProfit.Model;
using ImHereNonProfit.UI.ViewModel;
using ImHereNonProfit.UI.ViewModel.Add;

namespace ImHereNonProfit.UI
{
    public class Mocks
    {
        public static BasicUser BasicUserMock =  new BasicUser()
        {
            UserType = UsersTypes.Donor,
            FirstName = "Aviad",
            LastName = "Sachs",
            Email = "zaxy78@gmail.com",
        };

        public static MemberUser MemberMock = new MemberUser()
        {
            Id = 2,
            UserType = UsersTypes.Member,
            FirstName = "Aviad",
            LastName = "Sachs",
            Email = "zaxy78@gmail.com",
        };

        public static ExecutiveUser ExecutiveMock = new ExecutiveUser
        {
            FirstName = "Avi",
            LastName = "G",
            Email = "avi.g@gmail.com",
            UserType = UsersTypes.Executive
        };

        public static List<BasicUser> UsersListMock = new List<BasicUser> {BasicUserMock,MemberMock,ExecutiveMock};

        public static Committee CommitteeMockFull = new Committee { Name = "Volunteers", CurrentYearBudget = 10000 };

        public static List<CommitteeDocument> CommitteeDocumentsListMock = new List<CommitteeDocument>
            {
                new CommitteeDocument { Tags = "Minutes, August, Elections", LastModified = DateTime.Now, CommitteeId = 1, Committee = CommitteeMockFull, Id = 2},
                new CommitteeDocument { Tags = "Minutes, September, Report", LastModified = DateTime.Now - TimeSpan.FromDays(1), CommitteeId = 1, Committee = CommitteeMockFull, Id = 3},
                new CommitteeDocument { Tags = "Minutes, October", LastModified = DateTime.Now - TimeSpan.FromDays(2), CommitteeId = 1, Committee = CommitteeMockFull, Id = 4},
                new CommitteeDocument { Tags = "Report", LastModified = DateTime.Now - TimeSpan.FromDays(3), CommitteeId = 1, Committee = CommitteeMockFull, Id = 5},
            };

        public static List<CommitteeReport> CommitteeReportsListMock = new List<CommitteeReport>
            {
                new CommitteeReport { /*Tags = "Minutes, August, Elections", = DateTime.Now,*/ CommitteeId = 1, Committee = CommitteeMockFull, Id = 2},
                new CommitteeReport { /*Tags = "Minutes, September, Report", LastModified = DateTime.Now - TimeSpan.FromDays(1),*/ CommitteeId = 1, Committee = CommitteeMockFull, Id = 3},
                new CommitteeReport { /*Tags = "Minutes, October", LastModified = DateTime.Now - TimeSpan.FromDays(2),*/ CommitteeId = 1, Committee = CommitteeMockFull, Id = 4},
                new CommitteeReport { /*Tags = "Report", LastModified = DateTime.Now - TimeSpan.FromDays(3),*/ CommitteeId = 1, Committee = CommitteeMockFull, Id = 5},
            };

        public static List<CommitteeExpens> CommitteeExpensesListMock = new List<CommitteeExpens>
            {
                new CommitteeExpens { Amount = 150, Currency = "ILS", Date = DateTime.Now, CommitteeId = 1, ForCommittee = CommitteeMockFull, Id = 2},
                new CommitteeExpens { Amount = 99, Currency = "ILS", Date = DateTime.Now - TimeSpan.FromDays(1), CommitteeId = 1, ForCommittee = CommitteeMockFull, Id = 3},
                new CommitteeExpens { Amount = 338, Currency = "ILS", Date = DateTime.Now - TimeSpan.FromDays(2), CommitteeId = 1, ForCommittee = CommitteeMockFull, Id = 4},
                new CommitteeExpens { Amount = 200, Currency = "ILS", Date = DateTime.Now - TimeSpan.FromDays(3), CommitteeId = 1, ForCommittee = CommitteeMockFull, Id = 5},
            } ;

        public static List<MemberUser> MembersListMock = new List<MemberUser>
            {
                new MemberUser { FirstName = "Avi", LastName = "G", Email = "avi.g@gmail.com", UserType = UsersTypes.Executive},
                new MemberUser { FirstName = "Asaf", LastName = "R", Email = "asafr.b@gmail.com", UserType = UsersTypes.Member},
                new MemberUser { FirstName = "Aviad", LastName = "Sachs", Email = "aviad.modiin.@gmail.com", UserType = UsersTypes.Member},
                new MemberUser { FirstName = "Shahar", LastName = "Harel", Email = "shahr.g@gmail.com", UserType = UsersTypes.Member},
            };

        public static List<Donation> MemberDonationListMock = new List<Donation>
            {
                new Donation { Amount = 100, Currency = "ILS", Date = DateTime.Now, Id = 2},
                new Donation { Amount = 100, Currency = "ILS", Date = DateTime.Now - TimeSpan.FromDays(30),  Id = 3},
                new Donation { Amount = 100, Currency = "ILS", Date = DateTime.Now - TimeSpan.FromDays(60),  Id = 4},
                new Donation { Amount = 200, Currency = "ILS", Date = DateTime.Now - TimeSpan.FromDays(90),  Id = 5},
            };

        public static Committee CommitteeMock = new Committee {Name = "Events", CurrentYearBudget = 100};

        public static CommitteeViewModel MockCommittee => new CommitteeViewModel(CommitteeMockFull);
        public static CommitteeBudgetViewModel MockCommitteeBudget => new CommitteeBudgetViewModel(CommitteeMockFull);
        public static CommitteeDocsViewModel MockCommitteeDocs => new CommitteeDocsViewModel(CommitteeMockFull);
        public static CommitteeActivityViewModel MockCommitteeActivity => new CommitteeActivityViewModel(CommitteeMockFull);
        public static CommitteeManageViewModel MockCommitteeManage => new CommitteeManageViewModel(CommitteeMockFull);
        public static DonationsViewModel MockDonationsViewModel => new DonationsViewModel(BasicUserMock,false);
        public static UsersViewModel MockUsersViewModel => new UsersViewModel();
        public static NewDonationViewModel MockNewDonationVm => new NewDonationViewModel(null);
        public static NewDocViewModel MockNewDocVm => new NewDocViewModel(null, null);
        public static NewActivityViewModel MockNewActvityVm => new NewActivityViewModel(null, null);
        public static NewExpenssViewModel MockNewExpenssVm => new NewExpenssViewModel(null, null);
        public static NewCommitteMemberVm MockNewCmVm => new NewCommitteMemberVm(null,null );
        public static NewCommitteViewModel MockNewCommitteeVm => new NewCommitteViewModel(null);


        static Mocks()
        {
            CommitteeMockFull.CommitteeDocuments = CommitteeDocumentsListMock;
            CommitteeMockFull.CommitteeReports = CommitteeReportsListMock;
            CommitteeMockFull.CommitteeExpenses = CommitteeExpensesListMock;
            CommitteeMockFull.Members = MembersListMock;
            CommitteeMockFull.Chair = ExecutiveMock;

            BasicUserMock.Donations = MemberDonationListMock;
            foreach (var donation in BasicUserMock.Donations)
            {
                donation.Donor = BasicUserMock;
            }

            MemberMock.MemberOf = new List<Committee>
            {
                CommitteeMock,
                CommitteeMockFull,
                new Committee() {Name = "Literature"}
            };

            /*ExecutiveMock.MemberOf = new List<Committee>
            {
                CommitteeMock,
                CommitteeMockFull,
                new Committee() {Name = "Literature"}
            };*/
            
        }
    }


}

