using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImHereNonProfit.Model;

namespace ImHereNonProfit.UI.Utils
{
    class ModelExtensions
    {
        
    }

    public static class CommitteeExtensions
    {
        public static string Description(this Committee committee)
        {
            return $"{committee.Name} Commitee";
        }
    }

    public static class BasicUserExtensions
    {
        public static string FullName(this BasicUser user)
        {
            return $"{user?.FirstName} {user?.LastName}";
        }

        public static async Task<bool> RemoveFromAllCommitteesAsync(this BasicUser user, AppModelContainer appModel)
        {
            if (user.UserType < UsersTypes.Member) return true;
            try
            {
                var member = user as MemberUser;
                if (member == null) throw new InvalidCastException();

                appModel.Entry(member).State = EntityState.Unchanged;
                var memberOfCommittees = member.MemberOf.ToList();
                foreach (var committee in memberOfCommittees)
                {
                    committee.Members.Remove(member);
                    appModel.Entry(committee).State = EntityState.Modified;
                }
                await appModel.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
