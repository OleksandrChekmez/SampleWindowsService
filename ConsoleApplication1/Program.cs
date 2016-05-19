using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string ghostScriptPath = @"c:\Program Files (x86)\gs\gs9.19\bin\gswin32c.exe";
            string printerName = "Bullzip PDF Printer";
            string pdfFileName = @"e:\1.pdf";
            if (args.Length > 2)
            {
                ghostScriptPath = args[0];
                printerName = args[1];
                pdfFileName = args[2];
            }
            else
            {
                Console.WriteLine("params missing, runing with default params: ["+ ghostScriptPath+"] ["+ printerName+"] ["+ pdfFileName);
            }
            PrintPDF(ghostScriptPath, 1, printerName, pdfFileName);
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
    }
}
