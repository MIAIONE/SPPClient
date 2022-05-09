using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace M2.NSudo
{
    /// <summary>
    /// The utility class help you to load NSudo Shared Library.
    /// </summary>
    public class NSudoInstance
    {
        /// <summary>
        /// Reads data from the NSudo logging infrastructure.
        /// </summary>
        /// <returns>
        /// The content of the data from the NSudo logging infrastructure.
        /// </returns>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate IntPtr NSudoReadLogType();

        /// <summary>
        /// Reads data to the NSudo logging infrastructure.
        /// </summary>
        /// <param name="Sender">
        /// The sender name of the data.
        /// </param>
        /// <param name="Content">
        /// The content of the data.
        /// </param>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate void NSudoWriteLogType(
            string Sender,
            string Content);

        /// <summary>
        /// Creates a new process and its primary thread.
        /// </summary>
        /// <param name="UserModeType">
        /// A value from the NSUDO_USER_MODE_TYPE enumerated type that 
        /// identifies the user mode.
        /// </param>
        /// <param name="PrivilegesModeType">
        /// A value from the NSUDO_PRIVILEGES_MODE_TYPE enumerated type that 
        /// identifies the privileges mode.
        /// </param>
        /// <param name="MandatoryLabelType">
        /// A value from the NSUDO_MANDATORY_LABEL_TYPE enumerated type that 
        /// identifies the mandatory label.
        /// </param>
        /// <param name="ProcessPriorityClassType">
        /// A value from the NSUDO_PROCESS_PRIORITY_CLASS_TYPE enumerated type 
        /// that identifies the process priority class.
        /// </param>
        /// <param name="ShowWindowModeType">
        ///  A value from the NSUDO_SHOW_WINDOW_MODE_TYPE enumerated type that 
        ///  identifies the ShowWindow mode.
        /// </param>
        /// <param name="WaitInterval">
        /// The time-out interval for waiting the process, in milliseconds.
        /// </param>
        /// <param name="CreateNewConsole">
        /// If this parameter is TRUE, the new process has a new console, 
        /// instead of inheriting its parent's console (the default).
        /// </param>
        /// <param name="CommandLine">
        /// The command line to be executed. The maximum length of this string 
        /// is 32K characters, the module name portion of CommandLine is 
        /// limited to MAX_PATH characters.
        /// </param>
        /// <param name="CurrentDirectory">
        /// The full path to the current directory for the process.The string 
        /// can also specify a UNC path.If this parameter is nullptr, the new 
        /// process will the same current drive and directory as the calling 
        /// process. (This feature is provided primarily for shells that need 
        /// to start an application and specify its initial drive and working 
        /// directory.)
        /// </param>
        /// <returns>
        /// HRESULT. If the function succeeds, the return value is S_OK.
        /// </returns>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate int NSudoCreateProcessType(
            NSUDO_USER_MODE_TYPE UserModeType,
            NSUDO_PRIVILEGES_MODE_TYPE PrivilegesModeType,
            NSUDO_MANDATORY_LABEL_TYPE MandatoryLabelType,
            NSUDO_PROCESS_PRIORITY_CLASS_TYPE ProcessPriorityClassType,
            NSUDO_SHOW_WINDOW_MODE_TYPE ShowWindowModeType,
            uint WaitInterval,
            [MarshalAs(UnmanagedType.Bool)] bool CreateNewConsole,
            string CommandLine,
            string CurrentDirectory);

        DLLFromMemory dLL = null;
        DLLFromMemory DMdLL = null;
        /// <summary>
        /// Initialize the NSudoInstance.
        /// </summary>
        public NSudoInstance() 
        {
            DMdLL = new(SPPClient.Properties.Resources.NSudoDM);
            //DMdLL.MemoryCallEntryPoint();
            dLL = new(SPPClient.Properties.Resources.NSudoAPI);
            //dLL.MemoryCallEntryPoint();
        }

        /// <summary>
        /// Uninitialize the NSudoInstance.
        /// </summary>
        ~NSudoInstance()
        {
            if(dLL != null)
            {
                dLL.Dispose();
            }
            if(DMdLL != null)
            {
                DMdLL.Dispose();
            }
        }

      

        /// <summary>
        /// Creates a new process and its primary thread.
        /// </summary>
        /// <param name="UserModeType">
        /// A value from the NSUDO_USER_MODE_TYPE enumerated type that 
        /// identifies the user mode.
        /// </param>
        /// <param name="PrivilegesModeType">
        /// A value from the NSUDO_PRIVILEGES_MODE_TYPE enumerated type that 
        /// identifies the privileges mode.
        /// </param>
        /// <param name="MandatoryLabelType">
        /// A value from the NSUDO_MANDATORY_LABEL_TYPE enumerated type that 
        /// identifies the mandatory label.
        /// </param>
        /// <param name="ProcessPriorityClassType">
        /// A value from the NSUDO_PROCESS_PRIORITY_CLASS_TYPE enumerated type 
        /// that identifies the process priority class.
        /// </param>
        /// <param name="ShowWindowModeType">
        ///  A value from the NSUDO_SHOW_WINDOW_MODE_TYPE enumerated type that 
        ///  identifies the ShowWindow mode.
        /// </param>
        /// <param name="WaitInterval">
        /// The time-out interval for waiting the process, in milliseconds.
        /// </param>
        /// <param name="CreateNewConsole">
        /// If this parameter is TRUE, the new process has a new console, 
        /// instead of inheriting its parent's console (the default).
        /// </param>
        /// <param name="CommandLine">
        /// The command line to be executed. The maximum length of this string 
        /// is 32K characters, the module name portion of CommandLine is 
        /// limited to MAX_PATH characters.
        /// </param>
        /// <param name="CurrentDirectory">
        /// The full path to the current directory for the process.The string 
        /// can also specify a UNC path.If this parameter is nullptr, the new 
        /// process will the same current drive and directory as the calling 
        /// process. (This feature is provided primarily for shells that need 
        /// to start an application and specify its initial drive and working 
        /// directory.)
        /// </param>
        /// 


        public void CreateProcess(
            string CommandLine,
            NSUDO_USER_MODE_TYPE UserModeType= NSUDO_USER_MODE_TYPE.SYSTEM,
            NSUDO_PRIVILEGES_MODE_TYPE PrivilegesModeType= NSUDO_PRIVILEGES_MODE_TYPE.ENABLE_ALL_PRIVILEGES,
            NSUDO_MANDATORY_LABEL_TYPE MandatoryLabelType= NSUDO_MANDATORY_LABEL_TYPE.SYSTEM,
            NSUDO_PROCESS_PRIORITY_CLASS_TYPE ProcessPriorityClassType = NSUDO_PROCESS_PRIORITY_CLASS_TYPE.REALTIME,
            NSUDO_SHOW_WINDOW_MODE_TYPE ShowWindowModeType= NSUDO_SHOW_WINDOW_MODE_TYPE.DEFAULT,
            uint WaitInterval=0,
            bool CreateNewConsole=true,
            string CurrentDirectory=null)
        {
            
            if (dLL == null)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            NSudoCreateProcessType NSudoCreateProcessInstance =
               dLL.GetDelegateFromFuncName<NSudoCreateProcessType>(
                    "NSudoCreateProcess");
            if(CurrentDirectory == null)
            {
                CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\";
            }
             
            int hr = NSudoCreateProcessInstance(
                UserModeType,
                PrivilegesModeType,
                MandatoryLabelType,
                ProcessPriorityClassType,
                ShowWindowModeType,
                WaitInterval,
                CreateNewConsole,
                CommandLine,
                CurrentDirectory);
            if (hr != 0)
            {
                throw new ExternalException("-", hr);
            }
        }
    }
}
