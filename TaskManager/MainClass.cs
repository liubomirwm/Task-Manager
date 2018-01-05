using System;
using TaskManager.Service;
using TaskManager.Tools;
using TaskManager.View;

namespace TaskManager
{
    class MainClass
    {
        static internal bool exitProgram = false;

        static void Main(string[] args)
        {
            while (true)
            {
                if (AuthenticationService.LoggedUser == null)
                {
                    LoginView loginView = new LoginView();
                    loginView.Show();
                }
                if (AuthenticationService.LoggedUser != null && AuthenticationService.LoggedUser.IsAdmin) //the checks for null are to prevent stepping into if logged out
                {
                    AdminView adminView = new AdminView();
                    adminView.Show();
                }
                else if (AuthenticationService.LoggedUser != null)
                {
                    RegularUserView regularUserView = new RegularUserView();
                    regularUserView.Show();
                }

                if (MainClass.exitProgram == true)
                {
                    return;
                }
            }
        }
    }
}
