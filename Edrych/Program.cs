using System;
using System.Windows.Forms;

namespace Edrych
{
    static class Program
    {
        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        static void Main()
        {
            MainWindow main = null;
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                main = new MainWindow();
                Application.Run(main);
            }
            catch (Exception e)
            {
                MessageBox.Show("Well, THAT was unexpected. Here's everything I know, but I'm gonna close after you hit OK...\r\n\r\n" + e.ToString(), "Unexpected Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (main != null)
                    main.Dispose();
            }
        }
    }
}
