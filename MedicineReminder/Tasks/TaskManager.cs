using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace MedicineReminder.Tasks
{
    class TaskManager
    {
        /// <summary>
        /// Registers background tasks for this application
        /// </summary>
        public static void RegisterTasks()
        {
            //Check if there are tasks already registered
            var registeredTasks = BackgroundTaskRegistration.AllTasks.Select(kvp => kvp.Value).ToList();
            if (registeredTasks.Count() == 0)
            {
                // We never registered tasks; register them now
                var builder = new BackgroundTaskBuilder
                {
                    //Set task entery point
                    TaskEntryPoint = typeof(BackgroundTask).FullName
                };

                //Setting background task to trigger every 15 minutes
                builder.Name = typeof(TimeTrigger).Name;
                builder.SetTrigger(new TimeTrigger(15, false));
                registeredTasks.Add(builder.Register());
            }

            //Register progress/completed handlers for all tasks
            foreach (IBackgroundTaskRegistration regTask in registeredTasks)
            {
                regTask.Progress += BackgroundTaskProgress;
                regTask.Completed += BackgroundTaskCompleted;
            }

        }

        static void BackgroundTaskProgress(BackgroundTaskRegistration sender, BackgroundTaskProgressEventArgs args)
        {
            //Called by non-UI thread when app running/resumed; not called if terminated         
            Log("{0}% {1}", args.Progress, sender.Name);
        }

        static void BackgroundTaskCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            //Called by non-UI thread when app running/resumed; not called if terminated
            try
            {
                args.CheckResult(); //Throws an exception if background task completed event has reported an error
                Log("Complete: {0}", sender.Name);
            }
            catch (Exception ex)
            {
                Log("{0}: {1}", ex.GetType().ToString(), sender.Name);
            }
        }

        static private void Log(String format, params Object[] args)
        {
            System.Diagnostics.Debug.WriteLine(format, args);
        }

    }
}
