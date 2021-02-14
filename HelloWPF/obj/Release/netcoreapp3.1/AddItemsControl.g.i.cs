﻿#pragma checksum "..\..\..\AddItemsControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "CFA59EFC8E0A00567E27D94E47A8A83A5659A3C4"
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
    /// AddItemsControl
    /// </summary>
    public partial class AddItemsControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\AddItemsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock error_text;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\AddItemsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox barcode;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\AddItemsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox name;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\AddItemsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox printName;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\AddItemsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox mrp;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\AddItemsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox stock;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\AddItemsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid bottomGrid;
        
        #line default
        #line hidden
        
        
        #line 114 "..\..\..\AddItemsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar createProgress;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\..\AddItemsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button submitButton;
        
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
            System.Uri resourceLocater = new System.Uri("/TextileApp;component/additemscontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\AddItemsControl.xaml"
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
            
            #line 8 "..\..\..\AddItemsControl.xaml"
            ((TextileApp.AddItemsControl)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.keyEventHandler);
            
            #line default
            #line hidden
            return;
            case 2:
            this.error_text = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.barcode = ((System.Windows.Controls.TextBox)(target));
            
            #line 66 "..\..\..\AddItemsControl.xaml"
            this.barcode.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationEvent);
            
            #line default
            #line hidden
            
            #line 66 "..\..\..\AddItemsControl.xaml"
            this.barcode.GotKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.texboxFocusedEvent);
            
            #line default
            #line hidden
            
            #line 66 "..\..\..\AddItemsControl.xaml"
            this.barcode.GotMouseCapture += new System.Windows.Input.MouseEventHandler(this.texboxFocusedEvent);
            
            #line default
            #line hidden
            return;
            case 4:
            this.name = ((System.Windows.Controls.TextBox)(target));
            
            #line 70 "..\..\..\AddItemsControl.xaml"
            this.name.GotKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.texboxFocusedEvent);
            
            #line default
            #line hidden
            
            #line 70 "..\..\..\AddItemsControl.xaml"
            this.name.GotMouseCapture += new System.Windows.Input.MouseEventHandler(this.texboxFocusedEvent);
            
            #line default
            #line hidden
            return;
            case 5:
            this.printName = ((System.Windows.Controls.TextBox)(target));
            
            #line 74 "..\..\..\AddItemsControl.xaml"
            this.printName.GotKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.texboxFocusedEvent);
            
            #line default
            #line hidden
            
            #line 74 "..\..\..\AddItemsControl.xaml"
            this.printName.GotMouseCapture += new System.Windows.Input.MouseEventHandler(this.texboxFocusedEvent);
            
            #line default
            #line hidden
            return;
            case 6:
            this.mrp = ((System.Windows.Controls.TextBox)(target));
            
            #line 82 "..\..\..\AddItemsControl.xaml"
            this.mrp.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationEvent);
            
            #line default
            #line hidden
            
            #line 82 "..\..\..\AddItemsControl.xaml"
            this.mrp.GotKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.texboxFocusedEvent);
            
            #line default
            #line hidden
            
            #line 82 "..\..\..\AddItemsControl.xaml"
            this.mrp.GotMouseCapture += new System.Windows.Input.MouseEventHandler(this.texboxFocusedEvent);
            
            #line default
            #line hidden
            return;
            case 7:
            this.stock = ((System.Windows.Controls.TextBox)(target));
            
            #line 86 "..\..\..\AddItemsControl.xaml"
            this.stock.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationEvent);
            
            #line default
            #line hidden
            
            #line 86 "..\..\..\AddItemsControl.xaml"
            this.stock.GotKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.texboxFocusedEvent);
            
            #line default
            #line hidden
            
            #line 86 "..\..\..\AddItemsControl.xaml"
            this.stock.GotMouseCapture += new System.Windows.Input.MouseEventHandler(this.texboxFocusedEvent);
            
            #line default
            #line hidden
            return;
            case 8:
            this.bottomGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 9:
            this.createProgress = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 10:
            this.submitButton = ((System.Windows.Controls.Button)(target));
            
            #line 117 "..\..\..\AddItemsControl.xaml"
            this.submitButton.GotFocus += new System.Windows.RoutedEventHandler(this.submitFocusEvent);
            
            #line default
            #line hidden
            
            #line 117 "..\..\..\AddItemsControl.xaml"
            this.submitButton.Click += new System.Windows.RoutedEventHandler(this.submitButtonEvent);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

