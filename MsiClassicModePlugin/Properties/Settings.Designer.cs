﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.ComponentModel;

namespace MsiClassicModePlugin.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        [CategoryAttribute("Advanced options")]
        public bool ClassicApplicationPoolPipeline {
            get {
                return ((bool)(this["ClassicApplicationPoolPipeline"]));
            }
            set {
                this["ClassicApplicationPoolPipeline"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        [CategoryAttribute("Advanced options")]
        public bool NoEveryone {
            get {
                return ((bool)(this["NoEveryone"]));
            }
            set {
                this["NoEveryone"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        [CategoryAttribute("Advanced options")]
        public bool NoShares {
            get {
                return ((bool)(this["NoShares"]));
            }
            set {
                this["NoShares"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        [CategoryAttribute("Advanced options")]
        public bool NoShortcuts {
            get {
                return ((bool)(this["NoShortcuts"]));
            }
            set {
                this["NoShortcuts"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        [CategoryAttribute("Advanced options")]
        public bool SkipClickOnceDeployer {
            get {
                return ((bool)(this["SkipClickOnceDeployer"]));
            }
            set {
                this["SkipClickOnceDeployer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        [CategoryAttribute("Advanced options")]
        public bool NoEnvVar {
            get {
                return ((bool)(this["NoEnvVar"]));
            }
            set {
                this["NoEnvVar"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>Italian (Italy)</string>
  <string>Language Packages</string>
  <string>TaskBuilder Framework</string>
  <string>TaskBuilder Studio</string>
  <string>Mago4</string>
  <string>Mago.net</string>
</ArrayOfString>")]
        [CategoryAttribute("Advanced options")]
        [Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        public global::System.Collections.Specialized.StringCollection KeepFeatures {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["KeepFeatures"]));
            }
            set {
                this["KeepFeatures"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        [Browsable(false)]
        public bool UpgradeRequired {
            get {
                return ((bool)(this["UpgradeRequired"]));
            }
            set {
                this["UpgradeRequired"] = value;
            }
        }
    }
}
