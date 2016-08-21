using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImHereNonProfit.Model;
using ImHereNonProfit.UI.Messages;
using ImHereNonProfit.UI.ViewModel;

namespace ImHereNonProfit.UI.Services
{
    public class LoginService
    {
        public BasicUser LoggedUser { get; /*private*/ set; }
        public bool IsLoggedIn { get; private set; }

        public async Task<bool> TryLogin(String email, String password)
        {
            if (IsLoggedIn)
            {
                Debug.WriteLine($"Login Failed!  User already logged in [{LoggedUser?.Email}]");
                return false;
            }

            using (var appModel = new AppModelContainer())
            {
                try
                {
                    var userInDb = await appModel.BasicUsers?.Where
                                            (user => user.Email.Equals(email) && user.Password.Equals(password)).
                                            FirstOrDefaultAsync();

                    if (userInDb == null) throw new ObjectNotFoundException("Email and\\or Password wasn't found or matched in server");

                    App.MainLoginService.LoggedUser = userInDb;
                    IsLoggedIn = true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Login Failed! [{e.GetType()}]: {e.Message}");
                }
            }
            return IsLoggedIn;
        }

         

    }
}
