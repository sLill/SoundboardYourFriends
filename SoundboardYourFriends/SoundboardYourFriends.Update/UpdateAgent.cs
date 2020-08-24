using Newtonsoft.Json;
using SoundboardYourFriends.Update.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoundboardYourFriends.Update
{
    public class UpdateAgent
    {
        #region Events..
        public static event EventHandler UpdateComplete;
        #endregion Events..

        #region Methods..
        #region CheckForUpdates
        /// <summary>
        /// Returns release information for tagged versions ahead of the local application version
        /// </summary>
        /// <returns>Returns null if no updates are available</returns>
        public static async Task<Release> CheckForUpdatesAsync()
        {
            Release release = null;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "SoundboardYourFriends");
                    var response = await httpClient.GetStringAsync("http://api.github.com/repos/sLill/SoundboardYourFriends/releases");
                   
                    var remoteRelease = JsonConvert.DeserializeObject<List<Release>>(response).Max(x => x);
                    var localVersion = Assembly.GetExecutingAssembly().GetName().Version;
                    release = remoteRelease.Version > localVersion ? remoteRelease : null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not retrieve release info{Environment.NewLine}");
                    throw ex;
                }
            }

            return release;
        }
        #endregion CheckForUpdates

        #region UpdateApplicationAsync
        public static void UpdateApplicationAsync(Release updateInformation)
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    //{System.Net.DownloadProgressChangedEventArgs
                    webClient.DownloadProgressChanged += (sender, e) => 
                    {
                        double percentComplete = ((double)e.BytesReceived / (double)e.TotalBytesToReceive) * 100.0;
                        Console.Write($"\rDownload progress: {Math.Round(percentComplete, 1)}%");
                    };

                    webClient.DownloadFileCompleted += (sender, e) => 
                    { 
                        Console.WriteLine($"{Environment.NewLine}Download complete.");
                        UpdateComplete?.Invoke(null, EventArgs.Empty);
                    };

                    string executableDownloadUrl = updateInformation.Assets.First(asset => asset.Name.Contains("SoundboardYourFriends.exe")).BrowserDownloadUrl;
                    string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SoundboardYourFriends.exe");
                    
                    webClient.DownloadFileAsync(new Uri(executableDownloadUrl), filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not update application.{Environment.NewLine}");
                    throw ex;
                }
            }
        }
        #endregion UpdateApplicationAsync
        #endregion Methods..
    }
}
