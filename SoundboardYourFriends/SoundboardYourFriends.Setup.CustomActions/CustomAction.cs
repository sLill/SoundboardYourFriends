using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;

namespace SoundboardYourFriends.Setup.CustomActions
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult InstallVBCable(Session session)
        {
            session.Log("Begin InstallVBCable");
            var actionResult = ActionResult.Success;

            try
            {
                string VBCableSetupPath = Path.Combine(Regex.Replace(session["InstallDir"],
                    @"SoundboardYourFriends\\SoundboardYourFriends",
                    @"SoundboardYourFriends\\VBCable\\VBCABLE_Setup_x64.exe"));

                var VBCableProcess = Process.Start(VBCableSetupPath);
                VBCableProcess.WaitForExit();

                int exitCode = VBCableProcess.ExitCode;
                actionResult = VBCableProcess.ExitCode == 1 ? ActionResult.Success : ActionResult.UserExit;
            }
            catch (Exception ex)
            {
                actionResult = ActionResult.Failure;
                session.Log(ex.Message);
            }

            session.Log($"CustomAction InstallVBCable complete");
            return actionResult;
        }
    }
}
