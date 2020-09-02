using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace SoundboardYourFriends.Update
{
    /// <summary>
    /// This application looks at the FileVersion of the local filepath (-f) provided and compares it to the Release tag of the Github url provided (-u)
    /// If the local FileVersion is behind the remote release version, it will attempt to download and overwrite local with remote
    /// </summary>
    public class Program
    {
        #region Member Variables..
        private static string _localFilepath = string.Empty;
        private static string _sourceUrl = string.Empty;
        private static object _lockObject = new object();
        private static bool _updateInProgress = false;
        #endregion Member Variables..

        #region Main
        static void Main(string[] args)
        {
            LoggingAgent.Open();

            try
            {
                InitializeInstanceVariables(args);

                if (_localFilepath != null)
                {
                    LoggingAgent.WriteLine("Checking for updates..");
                    
                    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(_localFilepath);
                    var updateInformation = UpdateAgent.CheckForUpdatesAsync(new Version(fileVersionInfo.FileVersion)).Result;
                    if (updateInformation != null)
                    {
                        _updateInProgress = true;

                        // Close the application if it's already running
                        var process = Process.GetProcessesByName("SoundboardYourFriends");
                        process?.Cast<Process>().ToList().ForEach(process => process.Kill());

                        // Not a waiting function
                        UpdateAgent.UpdateApplicationAsync(updateInformation, _localFilepath);
                    }
                    else
                    {
                        LoggingAgent.WriteLine("No updates found.");
                        OnUpdateComplete(null, EventArgs.Empty);
                    }
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
                // Suppress application startup until the update/download completes
                while (_updateInProgress)
                { }

                // Start the main application
                Process.Start(_localFilepath, "-u");

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

        #region ParseCommandLineArguments
        private static Dictionary<string, string> ParseCommandLineArguments(string[] args)
        {
            var commandLineArgumentCollection = new Dictionary<string, string>();

            string argumentPattern = "(^|\\s|-)(?<FlagDescriptor>-\\S\\s)\\s?['\"]?(?<Argument>[\\\\/\\s\\S] +?)((?=\\s[-]\\S\\s)|['\"]|$)";
            Regex argumentRegex = new Regex(argumentPattern);
            args.Cast<string>().ToList().ForEach(argument =>
            {
                argumentRegex.Matches(argument).Cast<Match>().ToList().ForEach(match =>
                {
                    commandLineArgumentCollection[match.Groups["Flag"].Value] = match.Groups["Argument"].Value;
                });
            });

            return commandLineArgumentCollection;
        }
        #endregion ParseCommandLineArguments

        #region InitializeInstanceVariables
        private static void InitializeInstanceVariables(string[] args)
        {
            try
            {
                var commandLineArgumentCollection = ParseCommandLineArguments(args);
                _localFilepath = commandLineArgumentCollection["-f"];
                _sourceUrl = commandLineArgumentCollection["-u"];
            }
            catch
            {
                LoggingAgent.WriteLine("Error parsing command line arguments");
                throw;
            }

        }
        #endregion InitializeInstanceVariables
        #endregion Methods..
    }
}
