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

            string setupDirectory = session.CustomActionData["SetupDirectory"];
            string vbCableSetupPath = Path.Combine(setupDirectory, "VBCABLE_Setup_x64.exe");

            try
            {
                var vbCableProcess = new Process();
                vbCableProcess.StartInfo.UseShellExecute = true;
                vbCableProcess.StartInfo.FileName = vbCableSetupPath;
                vbCableProcess.StartInfo.Verb = "runas";

                vbCableProcess.Start();
                vbCableProcess.WaitForExit();

                session.Log($"CustomAction InstallVBCable completed successfully - ExitCode({vbCableProcess.ExitCode})");
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

            string setupDirectory = session.CustomActionData["SetupDirectory"];
            string dsoFileSetupPath = Path.Combine(setupDirectory, "DSOFile_Install.bat");

            try
            {
                var registerDSOFileProcess = new Process();
                registerDSOFileProcess.StartInfo.FileName = dsoFileSetupPath;
                registerDSOFileProcess.StartInfo.Verb = "runas";
                registerDSOFileProcess.StartInfo.CreateNoWindow = true;
                registerDSOFileProcess.StartInfo.UseShellExecute = true;

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
