﻿#pragma checksum "C:\Users\Acer\source\repos\PropertyManagement\PropertyManagement\MainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "339D3311FD6CD82EB1C8A3E5F83D6167D3E61680B05A7E88FFFD7C7E30D46A8B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PropertyManagement
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainPage.xaml line 32
                {
                    this.EmailTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 3: // MainPage.xaml line 33
                {
                    this.PasswordBox = (global::Windows.UI.Xaml.Controls.PasswordBox)(target);
                }
                break;
            case 4: // MainPage.xaml line 34
                {
                    this.LoginButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.LoginButton).Click += this.LoginButton_Click;
                }
                break;
            case 5: // MainPage.xaml line 35
                {
                    this.RegisterButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.RegisterButton).Click += this.RegisterButton_Click;
                }
                break;
            case 6: // MainPage.xaml line 36
                {
                    this.ForgotPasswordButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.ForgotPasswordButton).Click += this.ForgotPasswordButton_Click;
                }
                break;
            case 7: // MainPage.xaml line 37
                {
                    this.StatusTextBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

