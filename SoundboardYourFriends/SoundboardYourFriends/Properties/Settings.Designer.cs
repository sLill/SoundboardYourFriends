﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using NAudio.Mixer;
using System;

namespace SoundboardYourFriends.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.6.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection CaptureDeviceIds {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["CaptureDeviceIds"]));
            }
            set {
                this["CaptureDeviceIds"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection OutputDeviceIds {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["OutputDeviceIds"]));
            }
            set {
                this["OutputDeviceIds"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SoundboardSampleDirectory {
            get {
                return ((string)(this["SoundboardSampleDirectory"]));
            }
            set {
                this["SoundboardSampleDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string RecordHotKey {
            get {
                return ((string)(this["RecordHotKey"]));
            }
            set {
                this["RecordHotKey"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string GlobalKeyModifier {
            get {
                return ((string)(this["GlobalKeyModifier"]));
            }
            set {
                this["GlobalKeyModifier"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("7112000")]
        public int ByteSampleSize
        {
            get
            {
                return ((int)(this["ByteSampleSize"]));
            }
            set
            {
                this["ByteSampleSize"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection OutputDevicePlaybackTypeCollection
        {
            get
            {
                return ((global::System.Collections.Specialized.StringCollection)(this["OutputDevicePlaybackTypeCollection"]));
            }
            set
            {
                this["OutputDevicePlaybackTypeCollection"] = value;
            }
        }
    }
}
