﻿#pragma checksum "C:\Users\Acer\source\repos\PropertyManagement\PropertyManagement\AppointmentList.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "22D3C678B9AA11731ED22283B869D2EA85F4B4BF3F30CF8B8D96BDA057D0F226"
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
    partial class AppointmentList : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private static class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_Controls_TextBlock_Text(global::Windows.UI.Xaml.Controls.TextBlock obj, global::System.String value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = targetNullValue;
                }
                obj.Text = value ?? global::System.String.Empty;
            }
            public static void Set_Windows_UI_Xaml_Documents_Run_Text(global::Windows.UI.Xaml.Documents.Run obj, global::System.String value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = targetNullValue;
                }
                obj.Text = value ?? global::System.String.Empty;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private class AppointmentList_obj4_Bindings :
            global::Windows.UI.Xaml.IDataTemplateExtension,
            global::Windows.UI.Xaml.Markup.IDataTemplateComponent,
            global::Windows.UI.Xaml.Markup.IXamlBindScopeDiagnostics,
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IAppointmentList_Bindings
        {
            private global::PropertyManagement.Appointment dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);
            private bool removedDataContextHandler = false;

            // Fields for each control that has bindings.
            private global::System.WeakReference obj4;
            private global::Windows.UI.Xaml.Controls.TextBlock obj6;
            private global::Windows.UI.Xaml.Controls.TextBlock obj7;
            private global::Windows.UI.Xaml.Controls.TextBlock obj8;
            private global::Windows.UI.Xaml.Documents.Run obj9;
            private global::Windows.UI.Xaml.Documents.Run obj10;
            private global::Windows.UI.Xaml.Documents.Run obj11;
            private global::Windows.UI.Xaml.Documents.Run obj12;

            // Static fields for each binding's enabled/disabled state
            private static bool isobj6TextDisabled = false;
            private static bool isobj7TextDisabled = false;
            private static bool isobj8TextDisabled = false;
            private static bool isobj9TextDisabled = false;
            private static bool isobj10TextDisabled = false;
            private static bool isobj11TextDisabled = false;
            private static bool isobj12TextDisabled = false;

            public AppointmentList_obj4_Bindings()
            {
            }

            public void Disable(int lineNumber, int columnNumber)
            {
                if (lineNumber == 73 && columnNumber == 44)
                {
                    isobj6TextDisabled = true;
                }
                else if (lineNumber == 74 && columnNumber == 44)
                {
                    isobj7TextDisabled = true;
                }
                else if (lineNumber == 84 && columnNumber == 44)
                {
                    isobj8TextDisabled = true;
                }
                else if (lineNumber == 87 && columnNumber == 42)
                {
                    isobj9TextDisabled = true;
                }
                else if (lineNumber == 82 && columnNumber == 42)
                {
                    isobj10TextDisabled = true;
                }
                else if (lineNumber == 76 && columnNumber == 42)
                {
                    isobj11TextDisabled = true;
                }
                else if (lineNumber == 78 && columnNumber == 42)
                {
                    isobj12TextDisabled = true;
                }
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 4: // AppointmentList.xaml line 59
                        this.obj4 = new global::System.WeakReference((global::Windows.UI.Xaml.Controls.Grid)target);
                        break;
                    case 6: // AppointmentList.xaml line 73
                        this.obj6 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 7: // AppointmentList.xaml line 74
                        this.obj7 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 8: // AppointmentList.xaml line 84
                        this.obj8 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 9: // AppointmentList.xaml line 87
                        this.obj9 = (global::Windows.UI.Xaml.Documents.Run)target;
                        break;
                    case 10: // AppointmentList.xaml line 82
                        this.obj10 = (global::Windows.UI.Xaml.Documents.Run)target;
                        break;
                    case 11: // AppointmentList.xaml line 76
                        this.obj11 = (global::Windows.UI.Xaml.Documents.Run)target;
                        break;
                    case 12: // AppointmentList.xaml line 78
                        this.obj12 = (global::Windows.UI.Xaml.Documents.Run)target;
                        break;
                    default:
                        break;
                }
            }

            public void DataContextChangedHandler(global::Windows.UI.Xaml.FrameworkElement sender, global::Windows.UI.Xaml.DataContextChangedEventArgs args)
            {
                 if (this.SetDataRoot(args.NewValue))
                 {
                    this.Update();
                 }
            }

            // IDataTemplateExtension

            public bool ProcessBinding(uint phase)
            {
                throw new global::System.NotImplementedException();
            }

            public int ProcessBindings(global::Windows.UI.Xaml.Controls.ContainerContentChangingEventArgs args)
            {
                int nextPhase = -1;
                ProcessBindings(args.Item, args.ItemIndex, (int)args.Phase, out nextPhase);
                return nextPhase;
            }

            public void ResetTemplate()
            {
                Recycle();
            }

            // IDataTemplateComponent

            public void ProcessBindings(global::System.Object item, int itemIndex, int phase, out int nextPhase)
            {
                nextPhase = -1;
                switch(phase)
                {
                    case 0:
                        nextPhase = -1;
                        this.SetDataRoot(item);
                        if (!removedDataContextHandler)
                        {
                            removedDataContextHandler = true;
                            (this.obj4.Target as global::Windows.UI.Xaml.Controls.Grid).DataContextChanged -= this.DataContextChangedHandler;
                        }
                        this.initialized = true;
                        break;
                }
                this.Update_((global::PropertyManagement.Appointment) item, 1 << phase);
            }

            public void Recycle()
            {
            }

            // IAppointmentList_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
            }

            public void DisconnectUnloadedObject(int connectionId)
            {
                throw new global::System.ArgumentException("No unloadable elements to disconnect.");
            }

            public bool SetDataRoot(global::System.Object newDataRoot)
            {
                if (newDataRoot != null)
                {
                    this.dataRoot = (global::PropertyManagement.Appointment)newDataRoot;
                    return true;
                }
                return false;
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::PropertyManagement.Appointment obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_Title(obj.Title, phase);
                        this.Update_Description(obj.Description, phase);
                        this.Update_Location(obj.Location, phase);
                        this.Update_Status(obj.Status, phase);
                        this.Update_Duration(obj.Duration, phase);
                        this.Update_StartDate(obj.StartDate, phase);
                        this.Update_StartTime(obj.StartTime, phase);
                    }
                }
            }
            private void Update_Title(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // AppointmentList.xaml line 73
                    if (!isobj6TextDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj6, obj, null);
                    }
                }
            }
            private void Update_Description(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // AppointmentList.xaml line 74
                    if (!isobj7TextDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj7, obj, null);
                    }
                }
            }
            private void Update_Location(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // AppointmentList.xaml line 84
                    if (!isobj8TextDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj8, obj, null);
                    }
                }
            }
            private void Update_Status(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // AppointmentList.xaml line 87
                    if (!isobj9TextDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Documents_Run_Text(this.obj9, obj, null);
                    }
                }
            }
            private void Update_Duration(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // AppointmentList.xaml line 82
                    if (!isobj10TextDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Documents_Run_Text(this.obj10, obj, null);
                    }
                }
            }
            private void Update_StartDate(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // AppointmentList.xaml line 76
                    if (!isobj11TextDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Documents_Run_Text(this.obj11, obj, null);
                    }
                }
            }
            private void Update_StartTime(global::System.String obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    // AppointmentList.xaml line 78
                    if (!isobj12TextDisabled)
                    {
                        XamlBindingSetters.Set_Windows_UI_Xaml_Documents_Run_Text(this.obj12, obj, null);
                    }
                }
            }
        }
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // AppointmentList.xaml line 51
                {
                    this.AppointmentListView = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    ((global::Windows.UI.Xaml.Controls.ListView)this.AppointmentListView).SelectionChanged += this.AppointmentListView_SelectionChanged;
                }
                break;
            case 13: // AppointmentList.xaml line 36
                {
                    this.AddAppointmentButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.AddAppointmentButton).Click += this.AddAppointmentButton_Click;
                }
                break;
            case 14: // AppointmentList.xaml line 42
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
            switch(connectionId)
            {
            case 4: // AppointmentList.xaml line 59
                {                    
                    global::Windows.UI.Xaml.Controls.Grid element4 = (global::Windows.UI.Xaml.Controls.Grid)target;
                    AppointmentList_obj4_Bindings bindings = new AppointmentList_obj4_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(element4.DataContext);
                    element4.DataContextChanged += bindings.DataContextChangedHandler;
                    global::Windows.UI.Xaml.DataTemplate.SetExtensionInstance(element4, bindings);
                    global::Windows.UI.Xaml.Markup.XamlBindingHelper.SetDataTemplateComponent(element4, bindings);
                }
                break;
            }
            return returnValue;
        }
    }
}

