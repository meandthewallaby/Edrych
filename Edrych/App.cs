using System;
using System.ComponentModel;
using Edrych.Helpers;

namespace Edrych
{
    /// <summary>Singleton class which handles the state of UI items and events for other classes to fire upon UI events that affect multiple classes</summary>
    public sealed class App
    {
        #region Private/Global Variables

        private static bool _isCutEnabled;
        private static bool _isCopyEnabled;
        private static bool _isPasteEnabled;
        private static bool _isUndoEnabled;
        private static bool _isRedoEnabled;
        private static bool _isSelectAllEnabled;
        private static bool _isDatabaseDropDownEnabled;
        private static bool _isQueryMenuVisible;
        private static bool _isQueryConnectEnabled;
        private static bool _isQueryDisconnectEnabled;
        private static bool _loadingDatabases;

        #endregion

        #region Public Properties

        /// <summary>Whether or not the cut option is enabled.</summary>
        public static bool IsCutEnabled { get { return _isCutEnabled; } set { if (value != _isCutEnabled) { _isCutEnabled = value; NotifyPropertyChanged("IsCutEnabled"); } } }
        /// <summary>Whether or not the copy option is enabled.</summary>
        public static bool IsCopyEnabled { get { return _isCopyEnabled; } set { if (value != _isCopyEnabled) { _isCopyEnabled = value; NotifyPropertyChanged("IsCopyEnabled"); } } }
        /// <summary>Whether or not the paste option is enabled.</summary>
        public static bool IsPasteEnabled { get { return _isPasteEnabled; } set { if (value != _isPasteEnabled) { _isPasteEnabled = value; NotifyPropertyChanged("IsPasteEnabled"); } } }
        /// <summary>Whether or not the undo option is enabled.</summary>
        public static bool IsUndoEnabled { get { return _isUndoEnabled; } set { if (value != _isUndoEnabled) { _isUndoEnabled = value; NotifyPropertyChanged("IsUndoEnabled"); } } }
        /// <summary>Whether or not the redo option is enabled.</summary>
        public static bool IsRedoEnabled { get { return _isRedoEnabled; } set { if (value != _isRedoEnabled) { _isRedoEnabled = value; NotifyPropertyChanged("IsRedoEnabled"); } } }
        /// <summary>Whether or not the select all option is enabled.</summary>
        public static bool IsSelectAllEnabled { get { return _isSelectAllEnabled; } set { if (value != _isSelectAllEnabled) { _isSelectAllEnabled = value; NotifyPropertyChanged("IsSelectAllEnabled"); } } }
        /// <summary>Whether or not the database drop down menu is enabled.</summary>
        public static bool IsDatabaseDropDownEnabled { get { return _isDatabaseDropDownEnabled; } set { if (value != _isDatabaseDropDownEnabled) { _isDatabaseDropDownEnabled = value; NotifyPropertyChanged("IsDatabaseDropDownEnabled"); } } }
        /// <summary>Whether or not the query menu is visible.</summary>
        public static bool IsQueryMenuVisible { get { return _isQueryMenuVisible; } set { if (value != _isQueryMenuVisible) { _isQueryMenuVisible = value; NotifyPropertyChanged("IsQueryMenuVisible"); } } }
        /// <summary>Whether or not the query connect option is enabled.</summary>
        public static bool IsQueryConnectEnabled { get { return _isQueryConnectEnabled; } set { if (value != _isQueryConnectEnabled) { _isQueryConnectEnabled = value; NotifyPropertyChanged("IsQueryConnectEnabled"); } } }
        /// <summary>Whether or not the query disconnect option is enabled.</summary>
        public static bool IsQueryDisconnectEnabled { get { return _isQueryDisconnectEnabled; } set { if (value != _isQueryDisconnectEnabled) { _isQueryDisconnectEnabled = value; NotifyPropertyChanged("IsQueryDisconnectEnabled"); } } }
        /// <summary>Whether or not the app is loading databases.</summary>
        public static bool LoadingDatabases { get { return _loadingDatabases; } set { if (value != _loadingDatabases) { _loadingDatabases = value; } } }

        #endregion

        #region Public Events

        /// <summary>Event to notify of a property changing</summary>
        public static event PropertyChangedEventHandler PropertyChanged;
        /// <summary>Event to notify of the save action being requested</summary>
        public static event EventHandler Save;
        /// <summary>Event to notify of the save as action being requested</summary>
        public static event EventHandler SaveAs;
        /// <summary>Event to notify of the cut action being requested</summary>
        public static event EventHandler Cut;
        /// <summary>Event to notify of the copy action being requested</summary>
        public static event EventHandler Copy;
        /// <summary>Event to notify of the paste action being requested</summary>
        public static event EventHandler Paste;
        /// <summary>Event to notify of the undo action being requested</summary>
        public static event EventHandler Undo;
        /// <summary>Event to notify of the redo action being requested</summary>
        public static event EventHandler Redo;
        /// <summary>Event to notify of the select all action being requested</summary>
        public static event EventHandler SelectAll;
        /// <summary>Event to notify of the query connect action being requested</summary>
        public static event EventHandler QueryConnect;
        /// <summary>Event to notify of the query disconnect action being requested</summary>
        public static event EventHandler QueryDisonnect;
        /// <summary>Event to notify of the query connection being changed</summary>
        public static event ConnectionChangedEventHandler ConnectionChanged;
        /// <summary>Event to notify that the active query has changed</summary>
        public static event EventHandler ActiveQueryChanged;
        /// <summary>Event to notify that the user wants to switch databases</summary>
        public static event EventHandler SwitchDatabases;
        /// <summary>Event to notify that the active database has changed</summary>
        public static event ConnectionChangedEventHandler DatabaseChanged;

        #endregion

        #region Public Methods

        /// <summary>Trigger for the Save event</summary>
        public static void OnSave(object sender, EventArgs e)
        {
            if (Save != null)
                Save(sender, e);
        }

        /// <summary>Trigger for the SaveAs event</summary>
        public static void OnSaveAs(object sender, EventArgs e)
        {
            if (SaveAs != null)
                SaveAs(sender, e);
        }

        /// <summary>Trigger for the Cut event</summary>
        public static void OnCut(object sender, EventArgs e)
        {
            if (Cut != null)
                Cut(sender, e);
        }

        /// <summary>Trigger for the Copy event</summary>
        public static void OnCopy(object sender, EventArgs e)
        {
            if (Copy != null)
                Copy(sender, e);
        }

        /// <summary>Trigger for the Paste event</summary>
        public static void OnPaste(object sender, EventArgs e)
        {
            if (Paste != null)
                Paste(sender, e);
        }

        /// <summary>Trigger for the Undo event</summary>
        public static void OnUndo(object sender, EventArgs e)
        {
            if (Undo != null)
                Undo(sender, e);
        }

        /// <summary>Trigger for the Redo event</summary>
        public static void OnRedo(object sender, EventArgs e)
        {
            if (Redo != null)
                Redo(sender, e);
        }

        /// <summary>Trigger for the SelectAll event</summary>
        public static void OnSelectAll(object sender, EventArgs e)
        {
            if (SelectAll != null)
                SelectAll(sender, e);
        }

        /// <summary>Trigger for the QueryConnect event</summary>
        public static void OnQueryConnect(object sender, EventArgs e)
        {
            if (QueryConnect != null)
                QueryConnect(sender, e);
        }

        /// <summary>Trigger for the QueryDisconnect event</summary>
        public static void OnQueryDisconnect(object sender, EventArgs e)
        {
            if (QueryDisonnect != null)
                QueryDisonnect(sender, e);
        }

        /// <summary>Trigger for the ConnectionChanged event</summary>
        public static void OnConnectionChanged(object sender, ConnectionChangedEventArgs e)
        {
            if (ConnectionChanged != null)
                ConnectionChanged(sender, e);
        }

        /// <summary>Trigger for the ActiveQueryChanged event</summary>
        public static void OnActiveQueryChanged(object sender, EventArgs e)
        {
            if (ActiveQueryChanged != null)
                ActiveQueryChanged(sender, e);
        }

        /// <summary>Trigger for the SwitchDatabases event</summary>
        public static void OnSwitchDatabases(object sender, EventArgs e)
        {
            if (SwitchDatabases != null)
                SwitchDatabases(sender, e);
        }

        /// <summary>Trigger for the DatabaseChanged event</summary>
        public static void OnDatabaseChanged(object sender, ConnectionChangedEventArgs e)
        {
            if (DatabaseChanged != null)
                DatabaseChanged(sender, e);
        }

        #endregion

        #region Private Methods

        /// <summary>Trigger for the PropertyChanged event</summary>
        /// <param name="Property">Property that changed</param>
        private static void NotifyPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(new App(), new PropertyChangedEventArgs(Property));
        }

        #endregion
    }
}
