using System;
using System.IO;
using TaskManager.Entity;
using TaskManager.Tools;

namespace TaskManager.Repository
{
    public class TasksRepository : BaseRepository<Task>
    {
        public TasksRepository(string filePath) : base(filePath)
        {}

        public override void PopulateEntity(StreamReader sr, Task task)
        {
            task.Id = Convert.ToInt32(sr.ReadLine());
            task.Title = sr.ReadLine();
            task.Description = sr.ReadLine();
            task.EvaluatedTimeToComplete = Convert.ToInt32(sr.ReadLine());
            task.AssigneeId = Convert.ToInt32(sr.ReadLine());
            task.CreatorId = Convert.ToInt32(sr.ReadLine());
            task.CreationDate = Convert.ToDateTime(sr.ReadLine());
            task.LastEditDate = Convert.ToDateTime(sr.ReadLine());
            task.TaskState = (TaskState)Enum.Parse(typeof(TaskState), sr.ReadLine());
        }

        public override void WriteEntity(StreamWriter sw, Task task)
        {
            sw.WriteLine(task.Id);
            sw.WriteLine(task.Title);
            sw.WriteLine(task.Description);
            sw.WriteLine(task.EvaluatedTimeToComplete);
            sw.WriteLine(task.AssigneeId);
            sw.WriteLine(task.CreatorId);
            sw.WriteLine(task.CreationDate);
            sw.WriteLine(task.LastEditDate);
            sw.WriteLine(task.TaskState);
        }
    }
}
