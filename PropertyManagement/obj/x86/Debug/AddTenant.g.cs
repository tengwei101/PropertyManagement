﻿#pragma checksum "C:\Users\Acer\Downloads\PropertyManagement-main\PropertyManagement-main\PropertyManagement\AddTenant.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "B3726F9720768404E86718F06BB157265337F46118474122357919566DC23C8D"
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
    partial class AddTenant : 
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
            case 2: // AddTenant.xaml line 48
                {
                    this.SubmitButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.SubmitButton).Click += this.SubmitButton_Click;
                }
                break;
            case 3: // AddTenant.xaml line 49
                {
                    this.BackButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.BackButton).Click += this.BackButton_Click;
                }
                break;
            case 4: // AddTenant.xaml line 22
                {
                    this.NameTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 5: // AddTenant.xaml line 23
                {
                    this.EmailTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6: // AddTenant.xaml line 24
                {
                    this.PhoneNumberTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 7: // AddTenant.xaml line 25
                {
                    this.DateOfBirthDatePicker = (global::Windows.UI.Xaml.Controls.DatePicker)(target);
                }
                break;
            case 8: // AddTenant.xaml line 26
                {
                    this.LeaseStartDatePicker = (global::Windows.UI.Xaml.Controls.DatePicker)(target);
                }
                break;
            case 9: // AddTenant.xaml line 27
                {
                    this.LeaseEndDatePicker = (global::Windows.UI.Xaml.Controls.DatePicker)(target);
                }
                break;
            case 10: // AddTenant.xaml line 28
                {
                    this.RentAmountTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 11: // AddTenant.xaml line 42
                {
                    this.ContractFileButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.ContractFileButton).Click += this.ContractFileButton_Click;
                }
                break;
            case 12: // AddTenant.xaml line 43
                {
                    this.SelectedContractFileName = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 13: // AddTenant.xaml line 44
                {
                    this.ContractFileImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 14: // AddTenant.xaml line 30
                {
                    this.RentPaymentFrequencyComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 15: // AddTenant.xaml line 37
                {
                    this.IsActiveTenantCheckBox = (global::Windows.UI.Xaml.Controls.CheckBox)(target);
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

