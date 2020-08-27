﻿using Newtonsoft.Json;
using SoundboardYourFriends.Update.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
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
        public static async Task<Release> CheckForUpdatesAsync(Version currentVersion)
        {
            Release release = null;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "SoundboardYourFriends");
                    var response = await httpClient.GetStringAsync("http://api.github.com/repos/sLill/SoundboardYourFriends/releases");

                    var remoteRelease = JsonConvert.DeserializeObject<List<Release>>(response).Max(x => x);
                    release = remoteRelease.Version > currentVersion ? remoteRelease : null;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("rate limit exceeded"))
                    {
                        LoggingAgent.WriteLine($"Github Api rate limit exceeded for this hour{Environment.NewLine}");
                    }
                    else
                    {
                        LoggingAgent.WriteLine($"Could not retrieve release info{Environment.NewLine}");
                        throw ex;
                    }
                }
            }

            return release;
        }
        #endregion CheckForUpdates

        #region UpdateApplicationAsync
        public static void UpdateApplicationAsync(Release updateInformation, string localFilepath)
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadProgressChanged += (sender, e) =>
                    {
                        double percentComplete = ((double)e.BytesReceived / (double)e.TotalBytesToReceive) * 100.0;
                        LoggingAgent.Write($"\rDownload progress: {Math.Round(percentComplete, 1)}%");
                    };

                    webClient.DownloadFileCompleted += (sender, e) =>
                    {
                        LoggingAgent.WriteLine($"{Environment.NewLine}Download complete.");
                        UpdateComplete?.Invoke(null, EventArgs.Empty);
                    };

                    string executableDownloadUrl = updateInformation.Assets.First(asset => asset.Name.Contains("SoundboardYourFriends.exe")).BrowserDownloadUrl;
                    string filePath = Path.Combine(localFilepath);

                    webClient.DownloadFileAsync(new Uri(executableDownloadUrl), filePath);
                }
                catch (Exception ex)
                {
                    LoggingAgent.WriteLine($"Could not update application.{Environment.NewLine}");
                    throw ex;
                }
            }
        }
        #endregion UpdateApplicationAsync
        #endregion Methods..
    }
}
