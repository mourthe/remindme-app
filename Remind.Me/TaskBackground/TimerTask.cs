using System;
using System.Linq;
using System.Xml;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation.Geofencing;
using Windows.UI.Notifications;

namespace TaskBackground
{
    public sealed class TimerTask : IBackgroundTask
    {
        static string TaskName = "TimerTask";

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var bingS = new BingService();

            foreach (var b in bingS.ListNearbyPlaces)
            {
                GeofencesHelper.CreateGeofence(b.Key, b.Value.Position.Latitude, b.Value.Position.Longitude);
            }
        }

        public async static void Register()
        {
            if (!IsTaskRegistered())
            {
                var result = await BackgroundExecutionManager.RequestAccessAsync();
                var builder = new BackgroundTaskBuilder();

                builder.Name = TaskName;
                builder.TaskEntryPoint = typeof(LocationTask).FullName;
                builder.SetTrigger(new LocationTrigger(LocationTriggerType.Geofence));

                var condition = new SystemCondition(SystemConditionType.InternetAvailable);
                builder.AddCondition(condition);

                builder.Register();
            }
        }


        public static void Unregister()
        {
            var entry = BackgroundTaskRegistration.AllTasks.FirstOrDefault(kvp => kvp.Value.Name == TaskName);

            if (entry.Value != null)
                entry.Value.Unregister(true);
        }

        public static bool IsTaskRegistered()
        {
            var taskRegistered = false;
            var entry = BackgroundTaskRegistration.AllTasks.FirstOrDefault(kvp => kvp.Value.Name == TaskName);

            if (entry.Value != null)
                taskRegistered = true;

            return taskRegistered;
        }
    }
}
