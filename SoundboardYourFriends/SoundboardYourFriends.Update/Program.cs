using System;
using System.Diagnostics;
using System.Linq;

namespace SoundboardYourFriends.Update
{
    public class Program
    {
        #region Member Variables..
        private static object _lockObject = new object();
        private static bool _updateInProgress = false;
        #endregion Member Variables..

        #region Main
        static void Main(string[] args)
        {
            LoggingAgent.Open();

            try
            {
                LoggingAgent.WriteLine("Checking for updates..");

                string currentVersionParameter = args.FirstOrDefault() ?? "1.0.0.0";
                Version currentVersion = new Version(currentVersionParameter);

                var updateInformation = UpdateAgent.CheckForUpdatesAsync(currentVersion).Result;
                _updateInProgress = updateInformation != null;

                if (_updateInProgress)
                {
                    // Close the application if it's already running
                    var process = Process.GetProcessesByName("SoundboardYourFriends");
                    process.Cast<Process>().ToList().ForEach(process => process.Kill());

                    // Not a waiting function
                    UpdateAgent.UpdateApplicationAsync(updateInformation);
                }
                else
                {
                    LoggingAgent.WriteLine("No updates found.");
                    OnUpdateComplete(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                lock(_lockObject)
                {
                    _updateInProgress = false;
                }

                string message = $"{Environment.NewLine}Update failed: {ex.ToString()}";
                LoggingAgent.WriteLine(message);
            }
            finally
            {
                // Suppress application startup until the update download completes
                while (_updateInProgress)
                { }

                // Start the main application
                Process.Start("SoundboardYourFriends.exe", "-u");

                LoggingAgent.Close();
                Environment.Exit(0);
            }
        }
        #endregion Main

        #region Methods..
        #region Event Handlers..
        #region OnUpdateComplete
        private static void OnUpdateComplete(object sender, EventArgs e)
        {
            UpdateAgent.UpdateComplete -= OnUpdateComplete;

            LoggingAgent.WriteLine("Update complete.");
            LoggingAgent.WriteLine($"{Environment.NewLine}{Environment.NewLine}");

            lock(_lockObject)
            {
                _updateInProgress = false;
            }
        }
        #endregion OnUpdateComplete 
        #endregion Event Handlers..
        #endregion Methods..
    }
}
