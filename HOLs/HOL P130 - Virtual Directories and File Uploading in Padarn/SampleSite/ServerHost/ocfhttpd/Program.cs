//                                                                   
// Copyright (c) 2007 OpenNETCF Consulting, LLC                        
//                                                                     
// This software ("SOFTWARE") is governed by the OpenNETCF Shared      
// Source License ("LICENSE") and your use of the SOFTWARE constitutes 
// acceptance of this license. Subject to the restrictions detailed in 
// the LICENSE, you may use the Software for any commercial or         
// non-commercial purpose, including distributing derivative works.    
//                                                                     
// Details of the OpenNETCF Shared Source License can be found at:
// http://www.opennetcf.com/sharedsourcelicense.ocf
//
// The above copyright notice and this permission notice shall be      
// included in all copies or substantial portions of the SOFTWARE.     
//                                                                     
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,     
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF  
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND               
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS 
// BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN  
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN   
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE    
// SOFTWARE.                                                           
// 
namespace Padarn
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using OpenNETCF.Web.Server;
    using OpenNETCF.WindowsCE.Services;
    using System.Windows.Forms;
    using SampleSite;
  using OpenNETCF.Web.Configuration;
	using OpenNETCF.Threading;
	using OpenNETCF.Diagnostics;

    class Program
    {
        private static readonly string WS_RUNNING_MUTEX_NAME = "WEB_SERVER_RUNNING";
        private const string EXCEPTION_FILE_NAME = "padarnexceptionlog.txt";

        private static WebServer m_ws;

        public static WebServer WebServer
        {
            get { return Program.m_ws; }
            set { Program.m_ws = value; }
        }

        static void FatalErrorCheck()
        {
            string exceptionPath = Path.Combine(AppPath, EXCEPTION_FILE_NAME);

            if (File.Exists(exceptionPath))
            {
                if (MessageBox.Show("A Fatal Exception occurred during the last run of Padarn.  Would you like to view the Exception Information?",
                    "Fatal Shutdown",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    TextReader reader = File.OpenText(exceptionPath);
                    string text = reader.ReadToEnd();
                    reader.Close();

                    MessageBox.Show(text, "Fatal Exception Information");
                }

                File.Delete(exceptionPath);
            }
        }

        static void Main()
        {
            bool isNew;
            NamedMutex mutex = new NamedMutex(true, WS_RUNNING_MUTEX_NAME, out isNew);

            // if we're already running, don't try to start again
            if (!isNew) return;

            // check for previous fatal errors
            FatalErrorCheck();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
           
            // make sure any non-Padarn web server is shut down
            ServiceCollection services = ServiceCollection.GetServices();
            foreach (Service svc in services)
            {
                if (svc.Prefix.IndexOf("HTP") >= 0)
                {
                    svc.Unload();
                    break;
                }
            }

            //Run the host application
            Application.Run(new ControlForm());

            mutex.Close();
        }

        public static string AppPath
        {
            get
            {
                return Path.GetDirectoryName(
                      Assembly.GetExecutingAssembly().GetName().CodeBase);
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;

            // write it to a log file
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0} at {1}\r\n", ex.GetType().FullName, DateTime.Now.ToString("MM/dd/yy hh:mm:ss")));
            sb.Append(string.Format("Runtime {0} terminating\r\n", e.IsTerminating ? "true" : "false"));
            sb.Append(string.Format("Message: {0}\r\n", ex.Message));
            sb.Append(string.Format("Stack trace: {0}\r\n", ex.StackTrace));

            Trace2.WriteLine(sb.ToString());

            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            else
            {
                string exceptionPath = Path.Combine(AppPath, EXCEPTION_FILE_NAME);
                StreamWriter writer = File.Exists(exceptionPath) ? File.AppendText(exceptionPath) : File.CreateText(exceptionPath);

                writer.Write(sb.ToString());
                writer.Close();
            }
        }
    }
}