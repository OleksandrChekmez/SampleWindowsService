using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Runtime.InteropServices;

namespace CommandWindowsService
{
    public partial class TextCommandService : ServiceBase
    {
        
        public TextCommandService()
        {
            InitializeComponent();
            
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
                        
            // Set up a timer to trigger every minute.
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10000; // 10 seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {

            PrintPDF(@"c:\Program Files (x86)\gs\gs9.19\bin\gswin32c.exe", 1, "Bullzip PDF Printer", @"e:\1.pdf");
            // this.RunScript(@"c:\start.bat");
        }



        public static bool PrintPDF(string ghostScriptPath, int numberOfCopies, string printerName, string pdfFileName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = " -dPrinted -dBATCH -dNOPAUSE -dNOSAFER -q -dNumCopies=" + Convert.ToString(numberOfCopies) + " -sDEVICE=ljet4 -sOutputFile=\"\\\\spool\\" + printerName + "\" \"" + pdfFileName + "\" ";
            startInfo.FileName = ghostScriptPath;
            startInfo.UseShellExecute = false;

            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            //startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process process = null;
            try
            {
                process = Process.Start(startInfo);
                Console.WriteLine(process.StandardError.ReadToEnd() + process.StandardOutput.ReadToEnd());
                process.WaitForExit(30000);
                if (process.HasExited == false)
                    process.Kill();
                int exitcode = process.ExitCode;
                process.Close();
                return exitcode == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private void RunScript(string processFileName)
        {            
                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = @"/C c:\start.bat",
                    CreateNoWindow = true,
                    ErrorDialog = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden
                };


                var process = new Process();
                process.StartInfo = startInfo;
                process.Start();
         
         
        }

        protected override void OnContinue()
        {
            
        }

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public long dwServiceType;
            public ServiceState dwCurrentState;
            public long dwControlsAccepted;
            public long dwWin32ExitCode;
            public long dwServiceSpecificExitCode;
            public long dwCheckPoint;
            public long dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
    }
}
