﻿#pragma checksum "C:\Users\Acer\Downloads\PropertyManagement-main\PropertyManagement-main\PropertyManagement\EditMaintenanceRequest.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5D72DF706185DF4B9482EC8661889DBFE54D57AF20A03DBD0A4154F997A2B040"
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
    partial class EditMaintenanceRequest : 
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
            case 2: // EditMaintenanceRequest.xaml line 42
                {
                    this.SubmitButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.SubmitButton).Click += this.SubmitButton_Click;
                }
                break;
            case 3: // EditMaintenanceRequest.xaml line 43
                {
                    this.BackButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.BackButton).Click += this.BackButton_Click;
                }
                break;
            case 4: // EditMaintenanceRequest.xaml line 22
                {
                    this.TenantNameTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 5: // EditMaintenanceRequest.xaml line 23
                {
                    this.DescriptionTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6: // EditMaintenanceRequest.xaml line 24
                {
                    this.PriorityComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 7: // EditMaintenanceRequest.xaml line 29
                {
                    this.StatusComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 8: // EditMaintenanceRequest.xaml line 34
                {
                    this.SubmissionDatePicker = (global::Windows.UI.Xaml.Controls.DatePicker)(target);
                    ((global::Windows.UI.Xaml.Controls.DatePicker)this.SubmissionDatePicker).DateChanged += this.SubmissionDatePicker_DateChanged;
                }
                break;
            case 9: // EditMaintenanceRequest.xaml line 35
                {
                    this.CompletionDatePicker = (global::Windows.UI.Xaml.Controls.DatePicker)(target);
                    ((global::Windows.UI.Xaml.Controls.DatePicker)this.CompletionDatePicker).DateChanged += this.CompletionDatePicker_DateChanged;
                }
                break;
            case 10: // EditMaintenanceRequest.xaml line 37
                {
                    this.BrowseButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.BrowseButton).Click += this.BrowseButton_Click;
                }
                break;
            case 11: // EditMaintenanceRequest.xaml line 38
                {
                    this.PreviewImage = (global::Windows.UI.Xaml.Controls.Image)(target);
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

