using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace SoundboardYourFriends.Update
{
    public class Program
    {
        #region Member Variables..
        private static bool _errorRaised = false;
        #endregion Member Variables..

        #region Main
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Checking for updates..");
                UpdateAgent.UpdateComplete += OnUpdateComplete;

                string currentVersionParameter = args.FirstOrDefault() ?? "1.0.0.0";
                Version currentVersion = new Version(currentVersionParameter);

                var updateInformation = UpdateAgent.CheckForUpdatesAsync(currentVersion).Result;
                if (updateInformation != null)
                {
                    // Close the application if it's already running
                    var process = Process.GetProcessesByName("SoundboardYourFriends");
                    process.Cast<Process>().ToList().ForEach(process => process.Kill());

                    // Not a waiting function
                    UpdateAgent.UpdateApplicationAsync(updateInformation);
                }
                else
                {
                    Console.WriteLine("No updates found.");
                    OnUpdateComplete(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                string message = $"Error: {ex.ToString()}";
                Console.WriteLine(message);

                _errorRaised = true;
            }

            Console.ReadKey();
        }
        #endregion Main

        #region Methods..
        #region OnUpdateComplete
        private static void OnUpdateComplete(object sender, EventArgs e)
        {
            UpdateAgent.UpdateComplete -= OnUpdateComplete;

            // Start the main application
            Process.Start("SoundboardYourFriends.exe", "-u");

            if (_errorRaised)
            {
                Console.WriteLine($"{Environment.NewLine}Update failed.");
            }
            else
            {
                Console.WriteLine("Update complete.");
                Environment.Exit(0);
            }
        }
        #endregion OnUpdateComplete 
        #endregion Methods..
    }
}
