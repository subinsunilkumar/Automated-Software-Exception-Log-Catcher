using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exception_Catcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {   
            var dir = Path.Combine(@"C:\Program Files (x86)", "Exception Catcher");
            if (Directory.Exists(dir)==false)
            {
                Directory.CreateDirectory(dir);
            }

            var temp = Path.Combine(dir,"Temp");
            if (Directory.Exists(temp) == false)
            {
                Directory.CreateDirectory(temp);
            }
            var backup = Path.Combine(dir, "Backup");
            if (Directory.Exists(backup) == false)
            {
                Directory.CreateDirectory(backup);
            }

            var result = Path.Combine(dir, "Exception List.txt");
            if (File.Exists(result))
            {
                var dateTime = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_fff");
                var moveFile = Path.Combine(backup, $"Exception List {dateTime}.txt");
                File.Copy(result,moveFile,true);
            }
            var list = new List<string>();
            while (true)
            {
                Thread.Sleep(300000);
                var files = Directory.GetFiles(@"C:\ProgramData\IPETRONIK\IPEmotion 2021 R2 Beta (x64)\Trace");
                foreach (var file in files)
                {
                    var copyFile = Path.Combine(temp,Path.GetFileName(file));
                    File.Copy(file,copyFile,true);
                    var content = File.ReadAllLines(copyFile,Encoding.ASCII);
                    var tmpTxt = string.Empty;
                    for (int i = 0; i < content.Length; i++)
                    {
                        if (content[i].Contains("GlobalExceptionHandler"))
                        {
                            for (int j = i; j < content.Length; j++)
                            {
                                tmpTxt = tmpTxt + content[j];
                                if (content[i].Contains("	"))
                                {
                                    if (list.Contains(tmpTxt)==false)
                                    {
                                        list.Add(tmpTxt);
                                        //MessageBox.Show(tmpTxt);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                        
                    if (File.Exists(copyFile))
                    {
                        File.Delete(copyFile);
                    }
                    File.WriteAllLines(result,list);
                }
            }
        }
    }
}
