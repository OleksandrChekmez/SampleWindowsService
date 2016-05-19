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
                        
            // Set up a timer to trigger every minute.
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10000; // 10 seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
            
            
        }

        protected override void OnStop()
        {
            
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {

        
                PrintPDF(@"c:\Program Files\gs\gs9.19\bin\gswin64c.exe", 1, "Bullzip PDF Printer", @"e:\1.pdf");
            //PrintPDF(@"c:\Program Files (x86)\gs\gs9.19\bin\gswin32c.exe", 1, "Bullzip PDF Printer", @"e:\1.pdf");
            // this.RunScript(@"c:\start.bat");
        }



        public static bool PrintPDF(string ghostScriptPath, int numberOfCopies, string printerName, string pdfFileName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = " -sDEVICE=mswinpr2 -dBATCH -dNOPAUSE -dPrinted -dNOSAFER -dNOPROMPT -dQUIET -sOutputFile=\"\\\\spool\\" + printerName + "\" \"" + pdfFileName + "\" ";
            startInfo.FileName = ghostScriptPath;
            startInfo.UseShellExecute = false;

            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            //startInfo.CreateNoWindow = true;
            startInfo.LoadUserProfile = true;
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

       
    }
}
