using System;
using TaskManager.Tools;
using TaskManager.Entity;
using TaskManager.Repository;
using System.Collections.Generic;
using TaskManager.Service;

namespace TaskManager.View
{
    public class UserManagementView
    {
        public void Show()
        {
            UserManagementOption selectedOption = RenderMenu();
            switch (selectedOption)
            {
                case UserManagementOption.Add:
                    AddUser();
                    break;
                case UserManagementOption.View:
                    ViewUser();
                    break;
                case UserManagementOption.GetAll:
                    ViewAllUsers();
                    break;
                case UserManagementOption.Edit:
                    EditUser();
                    break;
                case UserManagementOption.Delete:
                    DeleteUser();
                    break;
                case UserManagementOption.Exit:
                    break;
                default:
                    throw new NotImplementedException("Reached default - this shouldn't happen in that case");
            }
        }

        public UserManagementOption RenderMenu()
        {
            while (true)
            {
                RenderMenu:
                Console.Clear();
                Console.WriteLine("[A]dd user");
                Console.WriteLine("[V]iew user");
                Console.WriteLine("[G]et all users");
                Console.WriteLine("[E]dit user/s");
                Console.WriteLine("[D]elete user");
                Console.WriteLine("E[x]it");
                Console.Write("Enter key to select option: ");
                string inputOption = Console.ReadLine();
                switch (inputOption.ToUpper())
                {
                    case "A":
                        return UserManagementOption.Add;
                    case "V":
                        return UserManagementOption.View;
                    case "G":
                        return UserManagementOption.GetAll;
                    case "E":
                        return UserManagementOption.Edit;
                    case "D":
                        return UserManagementOption.Delete;
                    case "X":
                        return UserManagementOption.Exit;
                    default:
                        Console.WriteLine("You have entered an invalid option. Try again with one of the available ones.");
                        Console.ReadKey(true);
                        goto RenderMenu;
                }
            }
        }

        public void AddUser()
        {
            Console.Clear();
            Console.WriteLine("Enter details about the new user below:");
            User user = new User();
            Console.Write("Fullname: ");
            user.FullName = Console.ReadLine();
            Console.Write("Username: ");
            user.Username = Console.ReadLine();
            if (user.Username.ToUpper() == "X")
            {
                Console.WriteLine("Username cannot be \"x\". Choose a different one!");
                Console.ReadKey(true);
                return;
            }

            Console.Write("Password: ");
            user.Password = Console.ReadLine();
            Console.Write("New user will be admin(true/false): ");
            bool isAdminTemp = false;
            bool isBool = bool.TryParse(Console.ReadLine(), out isAdminTemp);
            while (isBool == false)
            {
                Console.WriteLine("You can only choose TRUE or FALSE for the admin option");
                Console.Write("New user will be admin(true/false): ");
                isBool = bool.TryParse(Console.ReadLine(), out isAdminTemp);
            }
            user.IsAdmin = isAdminTemp;

            UsersRepository userRepo = new UsersRepository("users.txt");
            userRepo.Save(user);
            Console.WriteLine("New user saved successfully!");
            Console.ReadKey(true);
        }

        public void ViewUser()
        {
            Console.Clear();
            Console.Write("Enter user id to show: ");
            int id = 0;
            bool isInt = int.TryParse(Console.ReadLine(), out id);
            while (isInt == false)
            {
                Console.WriteLine("Only numbers are allowed for user ID's!!!");
                Console.Write("Enter user id to show: ");
                isInt = int.TryParse(Console.ReadLine(), out id);

            }

            UsersRepository userRepo = new UsersRepository("users.txt");
            List<User> users = userRepo.GetAll(User => User.Id == id);
            if (users.Count == 0)
            {
                Console.WriteLine("A user with that Id does not exist!");
                Console.ReadKey(true);
            }
            else
            {
                foreach (User user in users)
                {
                    Console.WriteLine("#########################");
                    Console.WriteLine($"Id: {user.Id}");
                    Console.WriteLine($"Full Name: {user.FullName}");
                    Console.WriteLine($"Username: {user.Username}");
                    Console.WriteLine($"Is Admin: {user.IsAdmin}");
                    Console.WriteLine("#########################");
                    Console.WriteLine();
                    Console.ReadKey(true);
                }
            }
        }

        public void ViewAllUsers()
        {
            UsersRepository userRepo = new UsersRepository("users.txt");
            List<User> users = userRepo.GetAll();
            foreach (User user in users)
            {
                Console.WriteLine("#########################");
                Console.WriteLine($"Id: {user.Id}");
                Console.WriteLine($"Full Name: {user.FullName}");
                Console.WriteLine($"Username: {user.Username}");
                Console.WriteLine($"Is Admin: {user.IsAdmin}");
                Console.WriteLine("#########################");
                Console.WriteLine();
            }
            Console.ReadKey(true);
        }

        public void EditUser()
        {
            Console.Clear();
            Console.Write("Enter user id to edit: ");
            int id = 0;
            bool IsInt = int.TryParse(Console.ReadLine(), out id);
            while (IsInt == false)
            {
                Console.WriteLine("Only numbers can be entered for ID's!!");
                Console.Write("Enter user id to edit: ");
                IsInt = int.TryParse(Console.ReadLine(), out id);
            }

            UsersRepository userRepo = new UsersRepository("users.txt");
            List<User> users = userRepo.GetAll(User => User.Id == id);
            if (users.Count == 0)
            {
                Console.WriteLine($"There is no user with id {id}!!!");
                Console.ReadKey(true);
                return;
            }
            User user = users[0];
            Console.WriteLine($"Old Full Name: {user.FullName}");
            Console.Write("New Full Name: ");
            user.FullName = Console.ReadLine();
            Console.WriteLine($"Old Username: {user.Username}");
            Console.Write("New Username: ");
            user.Username = Console.ReadLine();
            Console.Write("New password: ");
            user.Password = Console.ReadLine();
            if (user.Id != 1)
            {
                Console.WriteLine($"User is admin: {user.IsAdmin}");
                Console.Write("User will be admin: ");
                bool isAdminTemp = false;
                bool isBool = bool.TryParse(Console.ReadLine(), out isAdminTemp);
                while (isBool == false)
                {
                    Console.WriteLine("You can only choose TRUE or FALSE for the admin option");
                    isBool = bool.TryParse(Console.ReadLine(), out isAdminTemp);
                }
                user.IsAdmin = isAdminTemp;
                userRepo.Save(user);
                Console.WriteLine("User was edited successfully!");
                Console.ReadKey(true);
            }
        }

        public void DeleteUser()
        {
            Console.Clear();
            Console.Write("Enter user id to delete: ");
            int id = 0;
            bool isInt = int.TryParse(Console.ReadLine(), out id);
            while (isInt == false)
            {
                Console.WriteLine("Only numbers can be entered for user ID's");
                isInt = int.TryParse(Console.ReadLine(), out id);
            }

            UsersRepository userRepo = new UsersRepository("users.txt");
            if (userRepo.CheckEntityExistence(User => User.Id == id) == false)
            {
                Console.WriteLine("No user with that id exists!!");
                Console.ReadKey(true);
                return;
            }
            if (id == 1)
            {
                Console.WriteLine("You cannot delete the built-in administrator account!!");
                Console.ReadKey(true);
                return;
            }

            CommentsRepository commentRepo = new CommentsRepository("comments.txt");
            List<Comment> comments = commentRepo.GetAll(Comment => Comment.CreatorId == id);
            foreach (Comment comment in comments)
            {
                commentRepo.Delete(comment);
            }

            TasksRepository taskRepo = new TasksRepository("tasks.txt");
            List<Task> tasks = taskRepo.GetAll(Task => Task.CreatorId == id || Task.AssigneeId == id);
            foreach (Task task in tasks)
            {
                taskRepo.Delete(task);
            }

            List<User> users = userRepo.GetAll(User => User.Id == id);
            foreach (User user in users)
            {
                userRepo.Delete(user);
            }

            Console.WriteLine("User successfully deleted!");
            Console.ReadKey(true);

        }
    }
}
