﻿using System;
using System.Linq;
using System.Xml;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation.Geofencing;
using Windows.UI.Notifications;

namespace TaskBackground
{
    public sealed class LocationTask : IBackgroundTask
    {
        static string TaskName = "GeofenceUniversalAppLocationTask";

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get the information of the geofence(s) that have been hit
            var reports = GeofenceMonitor.Current.ReadReports();
            var report = reports.FirstOrDefault(r => /*(r.Geofence.Id == "casa") &&*/ (r.NewState == GeofenceState.Entered));

            if (report == null) return;

            // Create a toast notification to show a geofence has been hit
            var toastXmlContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            var txtNodes = toastXmlContent.GetElementsByTagName("text");            
            txtNodes[0].AppendChild(toastXmlContent.CreateTextNode("Você está perto de "));
            txtNodes[1].AppendChild(toastXmlContent.CreateTextNode(report.Geofence.Id));

            // add paramenters
            var toastNavigationString = "#/MainPage.xaml?param1=12345";

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

                // Uncomment this if your task requires an internet connection
                //var condition = new SystemCondition(SystemConditionType.InternetAvailable);
                //builder.AddCondition(condition);

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
