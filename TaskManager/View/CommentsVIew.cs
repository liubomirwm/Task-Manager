using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entity;
using TaskManager.Repository;
using TaskManager.Service;
using TaskManager.Tools;

namespace TaskManager.View
{
    public class CommentsView
    {
        public void Show()
        {
            Console.Clear();
            CommentsViewOption selectedOption = RenderMenu();
            switch (selectedOption)
            {
                case CommentsViewOption.AddComment:
                    AddComment();
                    break;
                case CommentsViewOption.ViewCreatedByMe:
                    ViewCommentsForTasksCreatedByMe();
                    break;
                case CommentsViewOption.ViewAssignedToMe:
                    ViewCommentsForTasksAssignedToMe();
                    break;
                case CommentsViewOption.Exit:
                    return;
                default:
                    throw new NotImplementedException("Reached default - this shouldn't happen in that case!!");
            }
        }

        public CommentsViewOption RenderMenu()
        {
            while (true)
            {
                RenderMenu:
                Console.Clear();
                Console.WriteLine("[A]dd comment");
                Console.WriteLine("[V]iew comments to tasks created by me");
                Console.WriteLine("[G]et comments to tasks assigned to me");
                Console.WriteLine("E[x]it");
                Console.Write("Enter key to select option: ");
                string inputOption = Console.ReadLine();
                switch (inputOption.ToUpper())
                {
                    case "A":
                        return CommentsViewOption.AddComment;
                    case "V":
                        return CommentsViewOption.ViewCreatedByMe;
                    case "G":
                        return CommentsViewOption.ViewAssignedToMe;
                    case "X":
                        return CommentsViewOption.Exit;
                    default:
                        Console.WriteLine("You have entered an invalid option. Choose one of the available ones.");
                        Console.ReadKey(true);
                        goto RenderMenu;
                }
            }
        }

        public void AddComment()
        {
            Console.Clear();
            Comment comment = new Comment();
            Console.Write("Enter id of the related task: ");
            int inputRelatedTaskId = 0;
            bool isInt = int.TryParse(Console.ReadLine(), out inputRelatedTaskId);
            while (isInt == false)
            {
                Console.WriteLine("Only integers can be entered for task ID's!!");
                Console.ReadKey();
                return;
            }
            TasksRepository taskRepo = new TasksRepository("tasks.txt");
            List<Entity.Task> tasks = taskRepo.GetAll(Task => Task.Id == inputRelatedTaskId);
            if (tasks.Count == 0)
            {
                Console.WriteLine($"No task with id {inputRelatedTaskId} exists!! ");
                Console.ReadKey(true);
                return;
            }
            Entity.Task task = tasks[0];
            if (!(task.AssigneeId == AuthenticationService.LoggedUser.Id || task.CreatorId == AuthenticationService.LoggedUser.Id))
            {
                Console.WriteLine("You can only add comments to tasks which you created or which you are assigned to!");
                Console.ReadKey(true);
                return;
            }
            
            comment.RelatedTaskId = inputRelatedTaskId;
            comment.CreatorId = AuthenticationService.LoggedUser.Id;
            Console.Write("Enter comment: ");
            comment.CommentText = Console.ReadLine();
            comment.CommentDate = DateTime.Now;
            CommentsRepository commentRepo = new CommentsRepository("comments.txt");
            commentRepo.Save(comment);
            Console.WriteLine("Comment saved successfully!");
            Console.ReadKey();
        }

        public void ViewCommentsForTasksCreatedByMe()
        {
            TasksRepository taskRepo = new TasksRepository("tasks.txt");
            List<Entity.Task> tasks = taskRepo.GetAll(Task => Task.CreatorId == AuthenticationService.LoggedUser.Id);
            if (tasks.Count == 0)
            {
                Console.WriteLine("There are no tasks created by you yet.");
                Console.ReadKey(true);
                return;
            }
            CommentsRepository commentRepo = new CommentsRepository("comments.txt");
            UsersRepository userRepo = new UsersRepository("users.txt");
            foreach (Entity.Task task in tasks)
            {
                Console.WriteLine();
                Console.WriteLine("#################################");
                Console.WriteLine($"Task id: {task.Id}");
                Console.WriteLine($"Task title: {task.Title}");
                List<Comment> comments = commentRepo.GetAll(Comment => Comment.RelatedTaskId == task.Id);
                if (comments.Count == 0)
                {
                    Console.WriteLine($"No comments for task {task.Id}");
                }
                else
                {
                    foreach (Comment comment in comments)
                    {
                        Console.WriteLine("*********************************");
                        Console.WriteLine($"Comment Id: {comment.Id}");
                        List<User> users = userRepo.GetAll(User => User.Id == comment.CreatorId);
                        Console.WriteLine($"Comment creator: {users[0].FullName}");
                        Console.WriteLine($"Commented on: {comment.CommentDate}");
                        Console.WriteLine($"Comment text: {comment.CommentText}");
                        Console.WriteLine("*********************************");
                    }
                }
                Console.WriteLine("#################################");
            }
            Console.ReadKey(true);
        }

        public void ViewCommentsForTasksAssignedToMe()
        {
            TasksRepository taskRepo = new TasksRepository("tasks.txt");
            List<Entity.Task> tasks = taskRepo.GetAll(Task => Task.AssigneeId == AuthenticationService.LoggedUser.Id);
            if (tasks.Count == 0)
            {
                Console.WriteLine("There are no tasks assigned to you yet.");
                Console.ReadKey(true);
                return;
            }
            CommentsRepository commentRepo = new CommentsRepository("comments.txt");
            UsersRepository userRepo = new UsersRepository("users.txt");
            foreach (Entity.Task task in tasks)
            {
                Console.WriteLine();
                Console.WriteLine("#################################");
                Console.WriteLine($"Task id: {task.Id}");
                Console.WriteLine($"Task title: {task.Title}");
                List<Comment> comments = commentRepo.GetAll(Comment => Comment.RelatedTaskId == task.Id);
                if (comments.Count == 0)
                {
                    Console.WriteLine($"No comments for task {task.Id}");
                }
                else
                {
                    foreach (Comment comment in comments)
                    {
                        Console.WriteLine("*********************************");
                        Console.WriteLine($"Comment Id: {comment.Id}");
                        List<User> users = userRepo.GetAll(User => User.Id == comment.CreatorId);
                        Console.WriteLine($"Comment creator: {users[0].FullName}");
                        Console.WriteLine($"Commented on: {comment.CommentDate}");
                        Console.WriteLine($"Comment text: {comment.CommentText}");
                        Console.WriteLine("*********************************");
                    }
                }
                Console.WriteLine("#################################");
            }
            Console.ReadKey(true);
        }
    }
}
