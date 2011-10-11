using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteBrowser
{
    public sealed class App
    {
        private static bool _isCopyEnabled;
        private static bool _isPasteEnabled;
        public static bool IsCopyEnabled { get { return _isCopyEnabled; } set { _isCopyEnabled = value; OnCopyEnabledChanged(); } }
        public static bool IsPasteEnabled { get { return _isPasteEnabled; } set { _isPasteEnabled = value; OnPasteEnabledChanged(); } }

        public static event EventHandler CopyEnabledChanged;
        private static void OnCopyEnabledChanged()
        {
            if (CopyEnabledChanged != null)
            {
                CopyEnabledChanged(null, new EventArgs());
            }
        }

        public static event EventHandler PasteEnabledChanged;
        private static void OnPasteEnabledChanged()
        {
            if (PasteEnabledChanged != null)
            {
                PasteEnabledChanged(null, new EventArgs());
            }
        }
    }
}
