using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Tools
{
    public enum TaskState
    {
        AwaitingExecution, InExecution, Finished
    }

    public enum AdminViewOption
    {
        ManageUsers, ManageTasks, Logout, ManageComments
    }

    public enum UserManagementOption
    {
        Add, View, GetAll, Edit, Delete, Exit
    }

    public enum TaskManagementOption
    {
        AddTask, EditTask, ViewCreatedTasks, GetAssignedTasks, DeleteTask, ChangeTaskState, Exit
    }

    public enum CommentsViewOption
    {
        AddComment, ViewCreatedByMe, ViewAssignedToMe, Exit
    }

    public enum RegularUserViewOption
    {
        ManageTasks, ManageComments, Logout
    }
}
