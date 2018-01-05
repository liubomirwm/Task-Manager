using System;
using TaskManager.Tools;
using TaskManager.Repository;
using TaskManager.Service;
using TaskManager.Entity;
using System.Collections.Generic;

namespace TaskManager.View
{
    public class TaskManagementView : BaseView
    {
        public void Show()
        {
            TaskManagementOption selectedOption = RenderMenu();
            switch (selectedOption)
            {
                case TaskManagementOption.AddTask:
                    AddTask();
                    break;
                case TaskManagementOption.EditTask:
                    EditTask();
                    break;
                case TaskManagementOption.ViewCreatedTasks:
                    ViewCreatedTasks();
                    break;
                case TaskManagementOption.GetAssignedTasks:
                    GetAssignedTasks();
                    break;
                case TaskManagementOption.DeleteTask:
                    DeleteTasks();
                    break;
                case TaskManagementOption.ChangeTaskState:
                    ChangeTaskState();
                    break;
                case TaskManagementOption.Exit:
                    return;
                default:
                    throw new NotImplementedException("Reached default - this shouldn't happen in that case");
            }
        }

        public TaskManagementOption RenderMenu()
        {
            while (true)
            {
                RenderMenu:
                Console.Clear();
                Console.WriteLine("[A]dd task");
                Console.WriteLine("[E]dit task");
                Console.WriteLine("[V]iew tasks created by me");
                Console.WriteLine("[G]et tasks assigned to me");
                Console.WriteLine("[D]elete task");
                Console.WriteLine("[C]hange task State");
                Console.WriteLine("E[x]it");
                Console.Write("Enter key to select option: ");
                string inputOption = Console.ReadLine();
                switch (inputOption.ToUpper())
                {
                    case "A":
                        return TaskManagementOption.AddTask;
                    case "E":
                        return TaskManagementOption.EditTask;
                    case "V":
                        return TaskManagementOption.ViewCreatedTasks;
                    case "G":
                        return TaskManagementOption.GetAssignedTasks;
                    case "D":
                        return TaskManagementOption.DeleteTask;
                    case "C":
                        return TaskManagementOption.ChangeTaskState;
                    case "X":
                        return TaskManagementOption.Exit;
                    default:
                        Console.WriteLine("You have entered an invalid option. Choose from the available ones above!!");
                        Console.ReadKey(true);
                        goto RenderMenu;
                }
            }
        }

        public void AddTask()
        {
            Entity.Task task = CollectTaskInformation();
            TasksRepository taskRepo = new TasksRepository("tasks.txt");
            taskRepo.Save(task);
            Console.WriteLine("Task saved successfully!!");
            Console.ReadKey(true);
        }

        public Entity.Task CollectTaskInformation()
        {
            Entity.Task task = new Entity.Task();
            Console.Clear();
            Console.Write("Enter task title: ");
            task.Title = Console.ReadLine();

            Console.Write("Enter description: ");
            task.Description = Console.ReadLine();

            Console.Write("Evaluated time to complete (in hours): ");
            int inputEvalTime = 0;
            bool isInt = int.TryParse(Console.ReadLine(), out inputEvalTime);
            while (isInt == false)
            {
                Console.WriteLine("Only integer numbers can be entered for hours!!");
                Console.Write("Evaluated time to complete (in hours): ");
                isInt = int.TryParse(Console.ReadLine(), out inputEvalTime);
            }
            task.EvaluatedTimeToComplete = inputEvalTime;

            inputAssigneeId:
            Console.Write("Enter id of the assignee: ");
            int inputAssigneeId = 0;
            bool isInt1 = int.TryParse(Console.ReadLine(), out inputAssigneeId);
            while (isInt1 == false)
            {
                Console.WriteLine("Only integers can be entered for ID's!!");
                Console.Write("Enter id of the assignee: ");
                isInt1 = int.TryParse(Console.ReadLine(), out inputAssigneeId);
            }
            UsersRepository userRepo = new UsersRepository("users.txt");
            if (userRepo.CheckEntityExistence(User => User.Id == inputAssigneeId) == false)
            {
                Console.WriteLine($"A user with id {inputAssigneeId} doesn't exist!!");
                Console.ReadKey(true);
                goto inputAssigneeId;
            }
            task.AssigneeId = inputAssigneeId;
            task.CreatorId = AuthenticationService.LoggedUser.Id;
            task.CreationDate = DateTime.Now;
            task.LastEditDate = DateTime.Now;
            Console.WriteLine("Enter one of the options below: \"A\" - Awaiting Execution; \"I\" - In Execution;");
            Console.Write("Task State: ");
            string inputTaskState = Console.ReadLine();
            switch (inputTaskState.ToUpper())
            {
                case "A":
                    task.TaskState = TaskState.AwaitingExecution;
                    break;
                case "I":
                    task.TaskState = TaskState.InExecution;
                    break;
                default:
                    Console.WriteLine("Invalid input!! Use one of the available options above!!");
                    Console.ReadKey(true);
                    break;
            }

            return task;
        }

        public void EditTask()
        {
            Console.Clear();
            Console.Write("Enter task id to edit: ");
            int id = 0;
            bool IsInt = int.TryParse(Console.ReadLine(), out id);
            while (IsInt == false)
            {
                Console.WriteLine("Only numbers can be entered for ID's!!");
                Console.Write("Enter task id to edit: ");
                IsInt = int.TryParse(Console.ReadLine(), out id);
            }

            TasksRepository taskRepo = new TasksRepository("tasks.txt");
            List<Task> tasks = taskRepo.GetAll(Task => Task.Id == id);
            if (tasks.Count == 0)
            {
                Console.WriteLine($"There is no task with id {id}!!!");
                Console.ReadKey(true);
                return;
            }
            Task task = tasks[0];
            Console.WriteLine($"Old title: {task.Title}");
            Console.Write("New title: ");
            task.Title = Console.ReadLine();

            Console.WriteLine($"Old description: {task.Description}");
            Console.Write("New description: ");
            task.Description = Console.ReadLine();

            Console.WriteLine($"Old evaluated time to complete: {task.EvaluatedTimeToComplete} hours");
            Console.Write("New evaluated time to complete: ");
            int tempEvalTime = 0;
            bool isInt = int.TryParse(Console.ReadLine(), out tempEvalTime);
            while (isInt == false)
            {
                Console.WriteLine("Only integers can be entered for the hours!!");
                Console.Write("New evaluated time to complete: ");
                isInt = int.TryParse(Console.ReadLine(), out tempEvalTime);
            }
            task.EvaluatedTimeToComplete = tempEvalTime;

            Console.WriteLine($"Old assignee id: {task.AssigneeId}");
            Console.Write("New assignee id: ");
            int tempAssigneeId = 0;
            bool isInt1 = int.TryParse(Console.ReadLine(), out tempAssigneeId);
            while (isInt1 == false)
            {
                Console.WriteLine("Only integers can be entered for the ID's!!");
                Console.Write("New assignee id: ");
                isInt1 = int.TryParse(Console.ReadLine(), out tempAssigneeId);
            }
            task.AssigneeId = tempAssigneeId;

            task.LastEditDate = DateTime.Now;

            Console.WriteLine($"Old task state: {task.TaskState}");
            Console.WriteLine("Enter one of the options below: \"A\" - Awaiting Execution; \"I\" - In Execution; \"F\" - Finished");
            Console.Write("New task State: ");
            string inputTaskState = Console.ReadLine();
            switch (inputTaskState.ToUpper())
            {
                case "A":
                    task.TaskState = TaskState.AwaitingExecution;
                    break;
                case "I":
                    task.TaskState = TaskState.InExecution;
                    break;
                case "F":
                    task.TaskState = TaskState.Finished;
                    break;
                default:
                    Console.WriteLine("Invalid input!! Use one of the available options above!!");
                    Console.ReadKey(true);
                    break;
            }

            taskRepo.Save(task);
            Console.WriteLine("Task edited successfully!!");
            Console.ReadKey(true);
        }

        public void ViewCreatedTasks()
        {
            TasksRepository taskRepo = new TasksRepository("tasks.txt");
            List<Task> tasks = taskRepo.GetAll(Task => Task.CreatorId == AuthenticationService.LoggedUser.Id);
            if (tasks.Count == 0)
            {
                Console.WriteLine("You haven't created any tasks yet.");
                Console.ReadKey(true);
            }
            else
            {
                UsersRepository userRepo = new UsersRepository("users.txt");
                foreach (Task task in tasks)
                {
                    Console.WriteLine("###########################");
                    Console.WriteLine($"Task id: {task.Id}");
                    Console.WriteLine($"Title: {task.Title}");
                    Console.WriteLine($"Description: {task.Description}");
                    Console.WriteLine($"Evaluated time to complete: {task.EvaluatedTimeToComplete} hours");
                    List<User> users = userRepo.GetAll(User => User.Id == task.AssigneeId);
                    Console.WriteLine($"Assignee: {users[0].FullName}");
                    users = userRepo.GetAll(User => User.Id == task.CreatorId);
                    Console.WriteLine($"Creator: {users[0].FullName}");
                    Console.WriteLine($"Creation date: {task.CreationDate}");
                    Console.WriteLine($"Last edit date: {task.LastEditDate}");
                    Console.WriteLine($"Task state: {task.TaskState}");
                    Console.WriteLine("###########################");
                }
                Console.ReadKey(true);
            }
        }

        public void GetAssignedTasks()
        {
            TasksRepository taskRepo = new TasksRepository("tasks.txt");
            List<Task> tasks = new List<Task>();
            tasks = taskRepo.GetAll(Task => Task.AssigneeId == AuthenticationService.LoggedUser.Id);
            if (tasks.Count == 0)
            {
                Console.WriteLine("You haven't been assigned any tasks yet.");
                Console.ReadKey(true);
            }
            else
            {
                UsersRepository userRepo = new UsersRepository("users.txt");
                foreach (Task task in tasks)
                {
                    Console.WriteLine("###########################");
                    Console.WriteLine($"Task id: {task.Id}");
                    Console.WriteLine($"Title: {task.Title}");
                    Console.WriteLine($"Description: {task.Description}");
                    Console.WriteLine($"Evaluated time to complete: {task.EvaluatedTimeToComplete} hours");
                    List<User> users = userRepo.GetAll(User => User.Id == task.AssigneeId);
                    Console.WriteLine($"Assignee: {users[0].FullName}");
                    users = userRepo.GetAll(User => User.Id == task.CreatorId);
                    Console.WriteLine($"Creator: {users[0].FullName}");
                    Console.WriteLine($"Creation date: {task.CreationDate}");
                    Console.WriteLine($"Last edit date: {task.LastEditDate}");
                    Console.WriteLine($"Task state: {task.TaskState}");
                    Console.WriteLine("###########################");
                }
                Console.ReadKey(true);
            }
        }

        public void DeleteTasks()
        {
            Console.Clear();
            Console.Write("Enter task id to delete: ");
            int inputId = 0;
            bool isInt = int.TryParse(Console.ReadLine(), out inputId);
            while (isInt == false)
            {
                Console.WriteLine("You can only enter integers for task ID's!!");
                Console.Write("Enter task id to delete: ");
                isInt = int.TryParse(Console.ReadLine(), out inputId);
            }

            TasksRepository taskRepo = new TasksRepository("tasks.txt");
            List<Task> tasks = taskRepo.GetAll(Task => Task.Id == inputId);
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks with that Id found.");
                Console.ReadKey(true);
            }
            else if (tasks[0].CreatorId != AuthenticationService.LoggedUser.Id)
            {
                Console.WriteLine("You cannot delete a task which was not created by you!!");
                Console.ReadKey(true);
            }
            else
            {
                foreach (Task task in tasks)
                {
                    taskRepo.Delete(task);
                }

                Console.WriteLine("Task successfully deleted!");
                Console.ReadKey(true);
            }
        }

        public void ChangeTaskState()
        {
            Console.Clear();
            Console.Write("Enter id of the task which State you would like to change: ");
            int inputTaskId = 0;
            bool isInt = int.TryParse(Console.ReadLine(), out inputTaskId);
            while (isInt == false)
            {
                Console.WriteLine("You can only enter integers for task ID's!!");
                Console.Write("Enter id of the task which state you would like to change: ");
                isInt = int.TryParse(Console.ReadLine(), out inputTaskId);
            }
            TasksRepository taskRepo = new TasksRepository("tasks.txt");
            List<Task> tasks = taskRepo.GetAll(Task => Task.Id == inputTaskId);
            if (tasks.Count == 0)
            {
                Console.WriteLine($"There are no tasks with id {inputTaskId}");
                Console.ReadKey(true);
            }

            Task task = tasks[0];
            if (!(task.AssigneeId == AuthenticationService.LoggedUser.Id || task.CreatorId == AuthenticationService.LoggedUser.Id))
            {
                Console.WriteLine("You can only modify task status of tasks which you created or which you are assigned to!");
                Console.ReadKey(true);
                return;
            }
            else
            {
                Console.WriteLine($"Current task State: {task.TaskState}");
                SelectTaskStatus:
                Console.WriteLine("Enter one of the options below: \"A\" - Awaiting Execution; \"I\" - In Execution; \"F\" - Finished");
                Console.Write("New task State: ");
                string inputTaskState = Console.ReadLine();
                switch (inputTaskState.ToUpper())
                {
                    case "A":
                        task.TaskState = TaskState.AwaitingExecution;
                        break;
                    case "I":
                        task.TaskState = TaskState.InExecution;
                        break;
                    case "F":
                        task.TaskState = TaskState.Finished;
                        break;
                    default:
                        Console.WriteLine("Invalid input!! Use one of the available options above!!");
                        Console.ReadKey(true);
                        goto SelectTaskStatus;
                }

                task.LastEditDate = DateTime.Now;
                taskRepo.Save(task);
                Console.WriteLine("Task State changed successfully!");
                Console.ReadKey(true);
            }
        }
    }
}
