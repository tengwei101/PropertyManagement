﻿#pragma checksum "C:\Users\Acer\source\repos\PropertyManagement\PropertyManagement\AddProperty.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "AB6BC39BCB50F2983E867EC1A66B3BAA9FCB077703DD6CA0D02D6401728DE2AD"
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
    partial class AddProperty : 
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
            case 2: // AddProperty.xaml line 14
                {
                    this.PropertyImageView = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 3: // AddProperty.xaml line 15
                {
                    this.UploadImageButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.UploadImageButton).Click += this.UploadImageButton_Click;
                }
                break;
            case 4: // AddProperty.xaml line 18
                {
                    this.PropertyNameTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 5: // AddProperty.xaml line 21
                {
                    this.PropertyTypeComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 6: // AddProperty.xaml line 30
                {
                    this.AddressTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 7: // AddProperty.xaml line 33
                {
                    this.BedroomsTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 8: // AddProperty.xaml line 36
                {
                    this.BathroomsTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 9: // AddProperty.xaml line 39
                {
                    this.SquareFeetTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 10: // AddProperty.xaml line 41
                {
                    this.DescriptionTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 11: // AddProperty.xaml line 44
                {
                    this.PropertyStatusComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 12: // AddProperty.xaml line 52
                {
                    this.OwnerTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 13: // AddProperty.xaml line 55
                {
                    this.PriceTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 14: // AddProperty.xaml line 58
                {
                    this.SubmitButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.SubmitButton).Click += this.SubmitButton_Click;
                }
                break;
            case 15: // AddProperty.xaml line 59
                {
                    this.BackButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.BackButton).Click += this.BackButton_Click;
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

