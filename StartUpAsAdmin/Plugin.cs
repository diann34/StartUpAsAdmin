
using ClassIsland.Core.Abstractions;
using ClassIsland.Core.Attributes;
using ClassIsland.Core.Extensions.Registry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace StartUpAsAdmin;

[PluginEntrance]
public class Plugin : PluginBase
{
    public override void Initialize(HostBuilderContext context, IServiceCollection services)
    {
        services.AddSettingsPage<StartUpAsAdminSettingsPage>();

        if (AdminHelper.IsRunningInAdmin() &&
            !File.Exists(Path.Combine(PluginConfigFolder, "migrated_1")) &&
            ScheduledTaskHelper.IsTaskCreated())
        {
            try
            {
                ScheduledTaskHelper.CreateScheduledTask();
                File.WriteAllText(Path.Combine(PluginConfigFolder, "migrated_1"), "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}