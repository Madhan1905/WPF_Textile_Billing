﻿#pragma checksum "..\..\..\LicenseControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "AD6360389238FCF13B39848518B8DF02224A9312"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using TextileApp;


namespace TextileApp {
    
    
    /// <summary>
    /// LicenseControl
    /// </summary>
    public partial class LicenseControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\LicenseControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ErrorLabel;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\LicenseControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label error_text;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\LicenseControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox filePath_text;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\LicenseControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button upload_button;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\LicenseControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button submit_button;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TextileApp;component/licensecontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\LicenseControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ErrorLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.error_text = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.filePath_text = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.upload_button = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\..\LicenseControl.xaml"
            this.upload_button.Click += new System.Windows.RoutedEventHandler(this.uploadEvent);
            
            #line default
            #line hidden
            return;
            case 5:
            this.submit_button = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\..\LicenseControl.xaml"
            this.submit_button.Click += new System.Windows.RoutedEventHandler(this.submitEvent);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

