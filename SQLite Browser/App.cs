using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private static bool _isQueryMenuVisible;
        private static bool _isQueryConnectEnabled;
        private static bool _isQueryDisconnectEnabled;

        public static bool IsCutEnabled { get { return _isCutEnabled; } set { if (value != _isCutEnabled) { _isCutEnabled = value; NotifyPropertyChanged("IsCutEnabled"); } } }
        public static bool IsCopyEnabled { get { return _isCopyEnabled; } set { if (value != _isCopyEnabled) { _isCopyEnabled = value; NotifyPropertyChanged("IsCopyEnabled"); } } }
        public static bool IsPasteEnabled { get { return _isPasteEnabled; } set { if (value != _isPasteEnabled) { _isPasteEnabled = value; NotifyPropertyChanged("IsPasteEnabled"); } } }
        public static bool IsUndoEnabled { get { return _isUndoEnabled; } set { if (value != _isUndoEnabled) { _isUndoEnabled = value; NotifyPropertyChanged("IsUndoEnabled"); } } }
        public static bool IsRedoEnabled { get { return _isRedoEnabled; } set { if (value != _isRedoEnabled) { _isRedoEnabled = value; NotifyPropertyChanged("IsRedoEnabled"); } } }
        public static bool IsSelectAllEnabled { get { return _isSelectAllEnabled; } set { if (value != _isSelectAllEnabled) { _isSelectAllEnabled = value; NotifyPropertyChanged("IsSelectAllEnabled"); } } }
        public static bool IsQueryMenuVisible { get { return _isQueryMenuVisible; } set { if (value != _isQueryMenuVisible) { _isQueryMenuVisible = value; NotifyPropertyChanged("IsQueryMenuVisible"); } } }
        public static bool IsQueryConnectEnabled { get { return _isQueryConnectEnabled; } set { if (value != _isQueryConnectEnabled) { _isQueryConnectEnabled = value; NotifyPropertyChanged("IsQueryConnectEnabled"); } } }
        public static bool IsQueryDisconnectEnabled { get { return _isQueryDisconnectEnabled; } set { if (value != _isQueryDisconnectEnabled) { _isQueryDisconnectEnabled = value; NotifyPropertyChanged("IsQueryDisconnectEnabled"); } } }

        public static event PropertyChangedEventHandler PropertyChanged;
        private static void NotifyPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(new App(), new PropertyChangedEventArgs(Property));
        }

        public static event EventHandler Save;
        public static void OnSave(object sender, EventArgs e)
        {
            if (Save != null)
                Save(sender, e);
        }

        public static event EventHandler SaveAs;
        public static void OnSaveAs(object sender, EventArgs e)
        {
            if (SaveAs != null)
                SaveAs(sender, e);
        }

        public static event EventHandler Cut;
        public static void OnCut(object sender, EventArgs e)
        {
            if (Cut != null)
                Cut(sender, e);
        }

        public static event EventHandler Copy;
        public static void OnCopy(object sender, EventArgs e)
        {
            if (Copy != null)
                Copy(sender, e);
        }

        public static event EventHandler Paste;
        public static void OnPaste(object sender, EventArgs e)
        {
            if (Paste != null)
                Paste(sender, e);
        }

        public static event EventHandler Undo;
        public static void OnUndo(object sender, EventArgs e)
        {
            if (Undo != null)
                Undo(sender, e);
        }

        public static event EventHandler Redo;
        public static void OnRedo(object sender, EventArgs e)
        {
            if (Redo != null)
                Redo(sender, e);
        }

        public static event EventHandler SelectAll;
        public static void OnSelectAll(object sender, EventArgs e)
        {
            if (SelectAll != null)
                SelectAll(sender, e);
        }

        public static event EventHandler QueryConnect;
        public static void OnQueryConnect(object sender, EventArgs e)
        {
            if (QueryConnect != null)
                QueryConnect(sender, e);
        }

        public static event EventHandler QueryDisonnect;
        public static void OnQueryDisconnect(object sender, EventArgs e)
        {
            if (QueryDisonnect != null)
                QueryDisonnect(sender, e);
        }
    }
}
