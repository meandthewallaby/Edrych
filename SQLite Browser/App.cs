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
        private static bool _isUndoEnabled;
        private static bool _isRedoEnabled;

        public static bool IsCopyEnabled { get { return _isCopyEnabled; } set { _isCopyEnabled = value; OnCopyEnabledChanged(); } }
        public static bool IsPasteEnabled { get { return _isPasteEnabled; } set { _isPasteEnabled = value; OnPasteEnabledChanged(); } }
        public static bool IsUndoEnabled { get { return _isUndoEnabled; } set { _isUndoEnabled = value; OnUndoEnabledChanged(); } }
        public static bool IsRedoEnabled { get { return _isRedoEnabled; } set { _isRedoEnabled = value; OnRedoEnabledChanged(); } }

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

        public static event EventHandler UndoEnabledChanged;
        private static void OnUndoEnabledChanged()
        {
            if (UndoEnabledChanged != null)
            {
                UndoEnabledChanged(null, new EventArgs());
            }
        }

        public static event EventHandler RedoEnabledChanged;
        private static void OnRedoEnabledChanged()
        {
            if (RedoEnabledChanged != null)
            {
                RedoEnabledChanged(null, new EventArgs());
            }
        }
    }
}
