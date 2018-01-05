using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Service;
using TaskManager.Tools;

namespace TaskManager.View
{
    public class RegularUserView
    {
        public void Show()
        {
            RegularUserViewOption selectedOption = RenderMenu();
            switch (selectedOption)
            {
                case RegularUserViewOption.ManageTasks:
                    TaskManagementView taskManagementView = new TaskManagementView();
                    taskManagementView.Show();
                    break;
                case RegularUserViewOption.ManageComments:
                    CommentsView commentsView = new CommentsView();
                    commentsView.Show();
                    break;
                case RegularUserViewOption.Logout:
                    AuthenticationService.LoggedUser = null;
                    return;
                default:
                    throw new NotImplementedException("Reached default: this shouldn't happen");
            }
        }

        public RegularUserViewOption RenderMenu()
        {
            RenderMenu:
            Console.Clear();
            Console.WriteLine("Manage [T]asks");
            Console.WriteLine("Manage [C]omments");
            Console.WriteLine("[L]ogout");
            Console.Write("Enter key to select option: ");
            string inputOption = Console.ReadLine();
            switch (inputOption.ToUpper())
            {
                case "T":
                    return RegularUserViewOption.ManageTasks;
                case "C":
                    return RegularUserViewOption.ManageComments;
                case "L":
                    return RegularUserViewOption.Logout;
                default:
                    Console.WriteLine("You have selected an invalid option! Choose one of the available ones!");
                    Console.ReadKey(true);
                    goto RenderMenu;
            }
        }
    }
}
