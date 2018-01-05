using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Service;
using TaskManager.Tools;

namespace TaskManager.View
{
    public class AdminView : BaseView
    {
        public void Show()
        {
            while (true)
            {
                AdminViewOption selectedOption = RenderMenu();
                switch (selectedOption)
                {
                    case AdminViewOption.ManageUsers:
                        UserManagementView userManagementView = new UserManagementView();
                        userManagementView.Show();
                        break;
                    case AdminViewOption.ManageTasks:
                        TaskManagementView taskManagementView = new TaskManagementView();
                        taskManagementView.Show();
                        break;
                    case AdminViewOption.ManageComments:
                        CommentsView commentsView = new CommentsView();
                        commentsView.Show();
                        break;
                    case AdminViewOption.Logout:
                        AuthenticationService.LoggedUser = null;
                        return;
                    default:
                        throw new NotImplementedException("Reached default - this shouldn't happen in that case");
                }
            }
        }

        public AdminViewOption RenderMenu()
        {
            while (true)
            {
                RenderMenu:
                Console.Clear();
                Console.WriteLine("Manage [U]sers");
                Console.WriteLine("Manage [T]asks");
                Console.WriteLine("Manage [C]omments");
                Console.WriteLine("[L]ogout");
                Console.Write("Enter key to select option: ");
                string inputOption = Console.ReadLine();
                switch (inputOption.ToUpper())
                {
                    case "U":
                        return AdminViewOption.ManageUsers;
                    case "T":
                        return AdminViewOption.ManageTasks;
                    case "C":
                        return AdminViewOption.ManageComments;
                    case "L":
                        return AdminViewOption.Logout;
                    default:
                        Console.WriteLine("You have entered an invalid option. Try again with one of the available ones.");
                        Console.ReadKey(true);
                        goto RenderMenu;
                }
            }
        }
    }
}
