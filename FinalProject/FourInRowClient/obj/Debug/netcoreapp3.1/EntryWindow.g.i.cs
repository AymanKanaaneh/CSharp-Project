#pragma checksum "..\..\..\EntryWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "13C6108C3C89DBC48078A663EA093CE4D491F8D7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FourInRowClient;
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


namespace FourInRowClient {
    
    
    /// <summary>
    /// EntryWindow
    /// </summary>
    public partial class EntryWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 103 "..\..\..\EntryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblName;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\..\EntryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label titleLbl;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\..\EntryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbGames;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\..\EntryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbWins;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\..\EntryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbLoses;
        
        #line default
        #line hidden
        
        
        #line 118 "..\..\..\EntryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbPoints;
        
        #line default
        #line hidden
        
        
        #line 119 "..\..\..\EntryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbWinsPercent;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\..\EntryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbUsers;
        
        #line default
        #line hidden
        
        
        #line 127 "..\..\..\EntryWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button clearBtn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FourInRowClient;V1.0.0.0;component/entrywindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\EntryWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\EntryWindow.xaml"
            ((FourInRowClient.EntryWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            
            #line 8 "..\..\..\EntryWindow.xaml"
            ((FourInRowClient.EntryWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.lblName = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.titleLbl = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            
            #line 106 "..\..\..\EntryWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.GameLbl_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.tbGames = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.tbWins = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.tbLoses = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.tbPoints = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.tbWinsPercent = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.lbUsers = ((System.Windows.Controls.ListBox)(target));
            
            #line 121 "..\..\..\EntryWindow.xaml"
            this.lbUsers.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lbUsers_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 123 "..\..\..\EntryWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnSearch);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 124 "..\..\..\EntryWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnStart);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 125 "..\..\..\EntryWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnExit);
            
            #line default
            #line hidden
            return;
            case 14:
            this.clearBtn = ((System.Windows.Controls.Button)(target));
            
            #line 127 "..\..\..\EntryWindow.xaml"
            this.clearBtn.Click += new System.Windows.RoutedEventHandler(this.btnClear);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

