using System;
using System.Windows.Forms;

namespace SQLiteBrowser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow());
            }
            catch (Exception e)
            {
                MessageBox.Show("Well, THAT was unexpected. Here's everything I know, but I'm gonna close after you hit OK...\r\n\r\n" + e.ToString(), "Unexpected Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
