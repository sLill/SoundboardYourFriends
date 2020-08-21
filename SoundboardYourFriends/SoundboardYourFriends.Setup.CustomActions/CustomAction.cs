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
                string solutionDirectory = session.CustomActionData["SolutionDirectory"];
                string VBCableSetupPath = Path.Combine(Regex.Replace(solutionDirectory,
                    @"SoundboardYourFriends\\SoundboardYourFriends",
                    @"SoundboardYourFriends\\VBCable\\VBCABLE_Setup_x64.exe"));

                var VBCableProcess = new Process();
                VBCableProcess.StartInfo.FileName = VBCableSetupPath;
                VBCableProcess.StartInfo.UseShellExecute = true;
                VBCableProcess.StartInfo.Verb = "runas";
                VBCableProcess.Start();
                VBCableProcess.WaitForExit();

                session.Log($"CustomAction InstallVBCable completed successfully - ExitCode({VBCableProcess.ExitCode})");
            }
            catch (Exception ex)
            {
                session.Log($"CustomAction InstallVBCable failed - {ex.Message}");
            }

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult RegisterDSOFileDll(Session session)
        {
            session.Log("Begin RegisterDSOFileDll");
            var actionResult = ActionResult.Success;

            try
            {
                string solutionDirectory = session.CustomActionData["SolutionDirectory"];
                string DSOFileSetupPath = Path.Combine(Regex.Replace(solutionDirectory,
                    @"SoundboardYourFriends\\SoundboardYourFriends",
                    @"SoundboardYourFriends\\DSOFile\\DSOFile_Install.bat"));

                var registerDSOFileProcess = new Process();
                registerDSOFileProcess.StartInfo.FileName = DSOFileSetupPath;
                registerDSOFileProcess.StartInfo.UseShellExecute = true;
                registerDSOFileProcess.StartInfo.Verb = "runas";
                registerDSOFileProcess.Start();
                registerDSOFileProcess.WaitForExit();

                actionResult = registerDSOFileProcess.ExitCode == 0 ? ActionResult.Success : ActionResult.UserExit;
                session.Log($"CustomAction RegisterDSOFileDll completed successfully - ExitCode({registerDSOFileProcess.ExitCode})");
            }
            catch (Exception ex)
            {
                session.Log($"CustomAction RegisterDSOFileDll failed - {ex.Message}");
                actionResult = ActionResult.Failure;
            }

            return actionResult;
        }
    }
}
