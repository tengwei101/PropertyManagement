﻿#pragma checksum "C:\Users\Acer\source\repos\PropertyManagement\PropertyManagement\TenantDetails.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "D56A96F2FA52C3B45656698242FD2FE000DD021CED3CC8C3C6647DE8AFBC0E5A"
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
    partial class TenantDetails : 
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
            case 2: // TenantDetails.xaml line 18
                {
                    this.TenantNameTextBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 3: // TenantDetails.xaml line 60
                {
                    global::Windows.UI.Xaml.Controls.Button element3 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element3).Click += this.DownloadFileButton_Click;
                }
                break;
            case 4: // TenantDetails.xaml line 61
                {
                    global::Windows.UI.Xaml.Controls.Button element4 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element4).Click += this.EditButton_Click;
                }
                break;
            case 5: // TenantDetails.xaml line 62
                {
                    global::Windows.UI.Xaml.Controls.Button element5 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element5).Click += this.DeleteButton_Click;
                }
                break;
            case 6: // TenantDetails.xaml line 63
                {
                    global::Windows.UI.Xaml.Controls.Button element6 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element6).Click += this.BackButton_Click;
                }
                break;
            case 7: // TenantDetails.xaml line 54
                {
                    this.ContractFileImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 8: // TenantDetails.xaml line 52
                {
                    this.ActiveCheckBox = (global::Windows.UI.Xaml.Controls.CheckBox)(target);
                    ((global::Windows.UI.Xaml.Controls.CheckBox)this.ActiveCheckBox).Checked += this.ActiveCheckBox_Checked;
                    ((global::Windows.UI.Xaml.Controls.CheckBox)this.ActiveCheckBox).Unchecked += this.ActiveCheckBox_Unchecked;
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

