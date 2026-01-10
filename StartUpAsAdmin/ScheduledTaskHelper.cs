using ClassIsland.Core;
using ClassIsland.Platforms.Abstraction;
using Microsoft.Win32.TaskScheduler;

namespace StartUpAsAdmin;

public static class ScheduledTaskHelper
{
    public const string TaskName = "ClassIsland.AdminStartup";
    
    public static void CreateScheduledTask()
    {
        var taskService = new TaskService();
        var task = taskService.NewTask();

        task.Triggers.Add(new LogonTrigger()
        {
            UserId = Environment.UserName
        });
        task.Actions.Add(new ExecAction()
        {
            Path = AppBase.ExecutingEntrance,
            WorkingDirectory = Path.GetDirectoryName(AppBase.ExecutingEntrance)
        });
        //task.Settings.RunOnlyIfLoggedOn = true;
        task.Settings.RunOnlyIfIdle = false;
        task.Settings.StopIfGoingOnBatteries = false;
        task.Settings.DisallowStartIfOnBatteries = false;
        task.Settings.ExecutionTimeLimit = TimeSpan.Zero;
        task.Principal.RunLevel = TaskRunLevel.Highest;
        taskService.RootFolder.RegisterTaskDefinition(TaskName, task, TaskCreation.CreateOrUpdate, null, null, TaskLogonType.InteractiveToken);
        PlatformServices.DesktopService.IsAutoStartEnabled = false;
    }

    public static void DeleteScheduledTask()
    {
        var taskService = new TaskService();
        taskService.RootFolder.DeleteTask(TaskName);
    }

    public static bool IsTaskCreated()
    {
        var taskService = new TaskService();
        return taskService.RootFolder.Tasks.Any(x => x.Name == TaskName);
    }
}