using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation.Geofencing;
using Windows.UI.Notifications;

namespace TaskBackground
{
    public sealed class LocationTask : IBackgroundTask
    {
        static string TaskName = "MyLocationTask";

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get the information of the geofence(s) that have been hit
            var reports = GeofenceMonitor.Current.ReadReports();

            GeofenceStateChangeReport report = null; 
            foreach (var r in reports)
                if (r.Geofence.Id == "MyGeofenceId" && r.NewState == GeofenceState.Entered)
                    report = r;
            
            if (report == null) return;

            // Create a toast notification to show a geofence has been hit
            var toastXmlContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            var txtNodes = toastXmlContent.GetElementsByTagName("text");
            // modificar para pegar o texto
            txtNodes[0].AppendChild(toastXmlContent.CreateTextNode("Geofence triggered toast!"));
            txtNodes[1].AppendChild(toastXmlContent.CreateTextNode(report.Geofence.Id));

            var toast = new ToastNotification(toastXmlContent);
            var toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toast);
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
