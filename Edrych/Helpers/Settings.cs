using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Edrych.DataAccess;

namespace Edrych.Helpers
{
    public class Settings
    {
        #region Constants

        private string SETTINGS_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Edrych/";
        private const string RECENT_CONNECTIONS_FILE = "RecentConnections.xml";

        #endregion

        #region Private/Global Variables

        private List<DataAccessConnection> _recentConnections;

        #endregion

        #region Constructor(s)
        public Settings()
        {
            EnsureFolderExists();
            LoadSettings();
        }
        #endregion

        #region Public Properties

        public List<DataAccessConnection> RecentConnections
        {
            get { return _recentConnections; }
        }

        #endregion

        #region Public Methods

        public void Save()
        {
            SaveRecentConnections();
        }

        #endregion

        #region Private Methods

        private void EnsureFolderExists()
        {
            if (!Directory.Exists(SETTINGS_PATH))
            {
                Directory.CreateDirectory(SETTINGS_PATH);
            }
        }

        private void LoadSettings()
        {
            GetRecentConnections();
        }

        //TODO: Try/Catch block around this to bubble up errors
        private void GetRecentConnections()
        {
            _recentConnections = new List<DataAccessConnection>();
            string filePath = SETTINGS_PATH + RECENT_CONNECTIONS_FILE;
            if (File.Exists(filePath))
            {
                XDocument doc = XDocument.Load(filePath);
                List<XElement> xmlConns = doc.Root.Elements().ToList();
                foreach (XElement conn in xmlConns)
                {
                    string connType = conn.Element("Type").Value;
                    string connSource = conn.Element("Source").Value;
                    ConnectionType type;
                    if (Enum.TryParse<ConnectionType>(connType, out type))
                    {
                        DataAccessConnection dac = new DataAccessConnection(type, connSource);
                        _recentConnections.Add(dac);
                    }
                }
            }

        }

        private void SaveRecentConnections()
        {
            XElement root = new XElement("Connections");
            foreach (DataAccessConnection conn in _recentConnections)
            {
                XElement xmlConn = new XElement(
                    "Connection",
                    new XElement("Type", conn.Connection.ToString()),
                    new XElement("Source", conn.DataSource)
                );
                root.Add(xmlConn);
            }

            root.Save(SETTINGS_PATH + RECENT_CONNECTIONS_FILE);
        }

        #endregion
    }
}
