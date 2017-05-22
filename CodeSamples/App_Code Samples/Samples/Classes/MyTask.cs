using System;

using CMS;
using CMS.EventLog;
using CMS.Scheduler;

// Custom scheduled task registration. Uncomment the following line to enable it.
//[assembly: RegisterCustomClass("Custom.MyTask", typeof(MyTask))]

/// <summary>
/// Sample task class.
/// </summary>
public class MyTask : ITask
{
    /// <summary>
    /// Executes the task.
    /// </summary>
    /// <param name="ti">Task info</param>
    public string Execute(TaskInfo ti)
    {
        EventLogProvider.LogEvent(EventType.INFORMATION, "MyTask", "Execute", "This task was executed from '~/App_Code/Samples/Classes/MyTask.cs'.", null);

        return null;
    }
}
