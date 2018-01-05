using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Service;

namespace TaskManager.View
{
    public class LoginView : BaseView
    {
        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Enter username (or \"x\" to close the program): ");
                string inputUsername = Console.ReadLine();
                if (inputUsername.ToUpper() == "X")
                {
                    MainClass.exitProgram = true;
                    return;
                }
                Console.Write("Enter password: ");
                string inputPassword = Console.ReadLine();

                AuthenticationService.AuthenticateUser(inputUsername, inputPassword);
                if (AuthenticationService.LoggedUser != null)
                {
                    Console.WriteLine($"Welcome {AuthenticationService.LoggedUser.FullName}");
                    Console.ReadKey(true);
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid username or password!!!");
                    Console.ReadKey(true);
                }

            }
        }
    }
}
