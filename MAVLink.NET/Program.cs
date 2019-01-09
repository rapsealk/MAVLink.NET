using System;
using System.Windows.Forms;

namespace MAVLink.NET
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            foreach (string portName in System.IO.Ports.SerialPort.GetPortNames())
            {
                Console.WriteLine("Port name: " + portName);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new Form1());
            }
            catch (ObjectDisposedException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
    }
}
