﻿#pragma checksum "..\..\..\HomeControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "BC5D99C1D48480E397CFD1077001D456E255E1A9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using HelloWPF;
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


namespace HelloWPF {
    
    
    /// <summary>
    /// HomeControl
    /// </summary>
    public partial class HomeControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 36 "..\..\..\HomeControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button billScreenButton;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\HomeControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button itemScreenButton;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\HomeControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button invoiceScreenButton;
        
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
            System.Uri resourceLocater = new System.Uri("/TextileApp;component/homecontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\HomeControl.xaml"
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
            this.billScreenButton = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\HomeControl.xaml"
            this.billScreenButton.Click += new System.Windows.RoutedEventHandler(this.billScreenButtonEvent);
            
            #line default
            #line hidden
            return;
            case 2:
            this.itemScreenButton = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\..\HomeControl.xaml"
            this.itemScreenButton.Click += new System.Windows.RoutedEventHandler(this.itemScreenButtonEvent);
            
            #line default
            #line hidden
            return;
            case 3:
            this.invoiceScreenButton = ((System.Windows.Controls.Button)(target));
            
            #line 46 "..\..\..\HomeControl.xaml"
            this.invoiceScreenButton.Click += new System.Windows.RoutedEventHandler(this.invoiceScreenButtonEvent);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 51 "..\..\..\HomeControl.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.quitEvent);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

