using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteBrowser
{
    public sealed class App
    {
        private static bool _isCutEnabled;
        private static bool _isCopyEnabled;
        private static bool _isPasteEnabled;
        private static bool _isUndoEnabled;
        private static bool _isRedoEnabled;
        private static bool _isSelectAllEnabled;

        public static bool IsCutEnabled { get { return _isCutEnabled; } set { _isCutEnabled = value; OnCutEnabledChanged(); } }
        public static bool IsCopyEnabled { get { return _isCopyEnabled; } set { _isCopyEnabled = value; OnCopyEnabledChanged(); } }
        public static bool IsPasteEnabled { get { return _isPasteEnabled; } set { _isPasteEnabled = value; OnPasteEnabledChanged(); } }
        public static bool IsUndoEnabled { get { return _isUndoEnabled; } set { _isUndoEnabled = value; OnUndoEnabledChanged(); } }
        public static bool IsRedoEnabled { get { return _isRedoEnabled; } set { _isRedoEnabled = value; OnRedoEnabledChanged(); } }
        public static bool IsSelectAllEnabled { get { return _isSelectAllEnabled; } set { _isSelectAllEnabled = value; OnSelectAllEnabledChanged(); } }

        public static event EventHandler CutEnabledChanged;
        private static void OnCutEnabledChanged()
        {
            if (CutEnabledChanged != null)
            {
                CutEnabledChanged(null, new EventArgs());
            }
        }

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

        public static event EventHandler SelectAllEnabledChanged;
        private static void OnSelectAllEnabledChanged()
        {
            if (SelectAllEnabledChanged != null)
            {
                SelectAllEnabledChanged(null, new EventArgs());
            }
        }

        public static event EventHandler Save;
        public static void OnSave(object sender, EventArgs e)
        {
            if (Save != null)
            {
                Save(null, new EventArgs());
            }
        }

        public static event EventHandler SaveAs;
        public static void OnSaveAs(object sender, EventArgs e)
        {
            if (SaveAs != null)
            {
                SaveAs(null, new EventArgs());
            }
        }

        public static event EventHandler Cut;
        public static void OnCut(object sender, EventArgs e)
        {
            if (Cut != null)
            {
                Cut(null, new EventArgs());
            }
        }

        public static event EventHandler Copy;
        public static void OnCopy(object sender, EventArgs e)
        {
            if (Copy != null)
            {
                Copy(null, new EventArgs());
            }
        }

        public static event EventHandler Paste;
        public static void OnPaste(object sender, EventArgs e)
        {
            if (Paste != null)
            {
                Paste(null, new EventArgs());
            }
        }

        public static event EventHandler Undo;
        public static void OnUndo(object sender, EventArgs e)
        {
            if (Undo != null)
            {
                Undo(null, new EventArgs());
            }
        }

        public static event EventHandler Redo;
        public static void OnRedo(object sender, EventArgs e)
        {
            if (Redo != null)
            {
                Redo(null, new EventArgs());
            }
        }

        public static event EventHandler SelectAll;
        public static void OnSelectAll(object sender, EventArgs e)
        {
            if (SelectAll != null)
            {
                SelectAll(null, new EventArgs());
            }
        }
    }
}
