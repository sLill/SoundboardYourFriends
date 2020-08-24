using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

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
                // Close the application if it's already running
                var process = Process.GetProcessesByName("SoundboardYourFriends");
                process.Cast<Process>().ToList().ForEach(process => process.Kill());

                Console.WriteLine("Checking for updates..");
                UpdateAgent.UpdateComplete += OnUpdateComplete;

                var updateInformation = UpdateAgent.CheckForUpdatesAsync().Result;
                if (updateInformation != null)
                {
                    // Not a waiting function
                    UpdateAgent.UpdateApplicationAsync(updateInformation);
                }
                else
                {
                    Console.WriteLine("No updates found.");
                }
            }
            catch (Exception ex)
            {
                string message = $"Error: {ex.ToString()}";
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
            string applicationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SoundboardYourFriends.exe");
            Process.Start(applicationPath, "-u");

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
