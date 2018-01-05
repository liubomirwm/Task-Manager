using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Tools;

namespace TaskManager.Entity
{
    public class Task : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int EvaluatedTimeToComplete { get; set; }
        public int AssigneeId { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastEditDate { get; set; }
        public TaskState TaskState { get; set; }
    }
}
