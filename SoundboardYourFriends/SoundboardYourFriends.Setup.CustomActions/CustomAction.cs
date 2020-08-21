using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.Linq;

namespace SoundboardYourFriends.Setup.CustomActions
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult InstallVBCable(Session session)
        {
            session.Log("Begin InstallVBCable");

            try
            {
                session.CustomActionData.Keys.Cast<string>().ToList().ForEach(x => session.Log($"CustomDataKeyValue : {x}"));

                string solutionDirectory = session.CustomActionData["SolutionDirectory"];

                session.Log($"CustomAction InstallVBCable exe path - {solutionDirectory}");
                string VBCableSetupPath = Path.Combine(Regex.Replace(solutionDirectory,
                    @"SoundboardYourFriends\\SoundboardYourFriends",
                    @"SoundboardYourFriends\\VBCable\\VBCABLE_Setup_x64.exe"));

                var VBCableProcess = new Process();
                VBCableProcess.StartInfo.FileName = VBCableSetupPath;
                VBCableProcess.StartInfo.UseShellExecute = true;
                VBCableProcess.StartInfo.Verb = "runas";
                VBCableProcess.Start();
                VBCableProcess.WaitForExit();

                session.Log($"CustomAction InstallVBCable completed successfully");
        }
            catch (Exception ex)
            {
                session.Log($"CustomAction InstallVBCable failed - {ex.Message}");
            }

            return ActionResult.Success;
        }
    }
}
