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

            string VBCableSetupPath = Path.Combine(Regex.Replace(Environment.CurrentDirectory,
                @"SoundboardYourFriends\\SoundboardYourFriends\\SoundboardYourFriends\.Setup\.CustomActions",
                @"VBCable\VBCABLE_Setup_x64.exe"));

            var VBCableProcess = Process.Start(VBCableSetupPath);
            int exitCode = VBCableProcess.ExitCode;

            return ActionResult.Success;
        }
    }
}
