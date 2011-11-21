using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using Edrych.DataAccess;

namespace Edrych.Helpers
{
    /// <summary>Class to contain the various application settings</summary>
    public class Settings
    {
        #region Constants

        private string SETTINGS_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Edrych/";
        private const string RECENT_CONNECTIONS_FILE = "RecentConnections.xml";

        #endregion

        #region Private/Global Variables

        private List<DataAccessConnection> _recentConnections;
        private List<DataAccessConnection> _odbcConnections;
        private byte[] entropy = System.Text.Encoding.ASCII.GetBytes("It was love at first sight. The first time Yossarian saw the chaplain, he fell madly in love with him.");

        #endregion

        #region Constructor(s)

        /// <summary>Initializes and loads the settings</summary>
        public Settings()
        {
            EnsureFolderExists();
            LoadSettings();
        }

        #endregion

        #region Public Properties

        /// <summary>List of recent connections</summary>
        public List<DataAccessConnection> RecentConnections
        {
            get { return _recentConnections; }
        }

        /// <summary>List of ODBC connections</summary>
        public List<DataAccessConnection> OdbcConnections
        {
            get { return _odbcConnections; }
        }

        #endregion

        #region Public Methods

        /// <summary>Public method to initiate settings to save</summary>
        public void Save()
        {
            SaveRecentConnections();
        }

        #endregion

        #region Private Methods

        /// <summary>Makes certain the Edrych folder exists in the user's AppData folder</summary>
        private void EnsureFolderExists()
        {
            if (!Directory.Exists(SETTINGS_PATH))
            {
                Directory.CreateDirectory(SETTINGS_PATH);
            }
        }

        /// <summary>Loads the recent and ODBC connections</summary>
        private void LoadSettings()
        {
            GetRecentConnections();
            GetOdbcConnections();
        }

        /// <summary>Loads the recent connections from an XML file</summary>
        private void GetRecentConnections()
        {
            _recentConnections = new List<DataAccessConnection>();
            XDocument doc = null;
            try
            {
                string filePath = SETTINGS_PATH + RECENT_CONNECTIONS_FILE;

                if (File.Exists(filePath))
                {
                    doc = XDocument.Load(filePath);
                    List<XElement> xmlConns = doc.Root.Elements().ToList();
                    foreach (XElement conn in xmlConns)
                    {
                        DataAccessConnection dac = new DataAccessConnection();
                        ConnectionType type;
                        AuthType auth;

                        foreach (XElement element in conn.Elements())
                        {
                            switch (element.Name.ToString())
                            {
                                case "Type":
                                    if (Enum.TryParse<ConnectionType>(element.Value, out type))
                                        dac.Connection = type;
                                    break;
                                case "Source":
                                    dac.DataSource = element.Value;
                                    break;
                                case "Auth":
                                    if (Enum.TryParse<AuthType>(element.Value, out auth))
                                        dac.Auth = auth;
                                    break;
                                case "Database":
                                    dac.Database = element.Value;
                                    break;
                                case "Username":
                                    dac.Username = element.Value;
                                    break;
                                case "Password":
                                    if(!string.IsNullOrEmpty(element.Value))
                                        dac.Password = ToInsecureString(DecryptString(element.Value));
                                    break;
                                default:
                                    break;
                            }
                        }
                        _recentConnections.Add(dac);
                    }
                }
            }
            catch
            {
                _recentConnections.Clear();
            }
        }

        /// <summary>Loads user and system DSN entries into the ODBC connections list</summary>
        private void GetOdbcConnections()
        {
            _odbcConnections = new List<DataAccessConnection>();
            GetOdbcFromRegistry(Registry.CurrentUser);
            GetOdbcFromRegistry(Registry.LocalMachine);
        }

        /// <summary>Saves the recent connections to an XML file</summary>
        private void SaveRecentConnections()
        {
            Dictionary<ConnectionType, int> numConnections = new Dictionary<ConnectionType, int>();
            XElement root = new XElement("Connections");
            foreach (DataAccessConnection conn in _recentConnections)
            {
                XElement xmlConn = new XElement(
                    "Connection"
                );
                xmlConn.Add(CreateElement("Type", conn.Connection.ToString()));
                xmlConn.Add(CreateElement("Source", conn.DataSource));
                xmlConn.Add(CreateElement("Database", conn.Database));
                xmlConn.Add(CreateElement("Auth", conn.Auth.ToString()));
                xmlConn.Add(CreateElement("Username", conn.Username));
                xmlConn.Add(CreateElement("Password", conn.Password, true));
                if (numConnections.ContainsKey(conn.Connection))
                {
                    if (numConnections[conn.Connection] < 5)
                    {
                        numConnections[conn.Connection]++;
                        root.Add(xmlConn);
                    }
                }
                else
                {
                    numConnections.Add(conn.Connection, 1);
                    root.Add(xmlConn);
                }
            }

            root.Save(SETTINGS_PATH + RECENT_CONNECTIONS_FILE);
        }

        /// <summary>One override of the CreateElement method for unencrypted strings</summary>
        /// <param name="Name">Name of the element</param>
        /// <param name="Value">Value of the element</param>
        /// <returns>XElement for the Name node</returns>
        private XElement CreateElement(string Name, string Value)
        {
            return CreateElement(Name, Value, false);
        }

        /// <summary>Creates an element for us in an XML file</summary>
        /// <param name="Name">Name of the node</param>
        /// <param name="Value">Value of the node</param>
        /// <param name="Encrypt">Whether or not to encrypt the value of the node</param>
        /// <returns>XElement</returns>
        private XElement CreateElement(string Name, string Value, bool Encrypt)
        {
            XElement element = new XElement(Name);
            if (!string.IsNullOrEmpty(Value))
            {
                if (Encrypt)
                {
                    element.Value = EncryptString(ToSecureString(Value));
                }
                else
                {
                    element.Value = Value;
                }
            }
            return element;
        }

        #endregion

        #region Private Methods - Registry

        /// <summary>Loads the ODBC DSN entries from the registry given the starting key</summary>
        /// <param name="key">Registry key representing the top level registry to look into</param>
        private void GetOdbcFromRegistry(RegistryKey key)
        {
            try
            {
                RegistryKey reg = key.OpenSubKey("SOFTWARE").OpenSubKey("ODBC").OpenSubKey("ODBC.INI");
                foreach (string dsn in reg.GetSubKeyNames().Where(k => k != "ODBC Data Sources"))
                {
                    RegistryKey dsnKey = reg.OpenSubKey(dsn);
                    DataAccessConnection dac = new DataAccessConnection();
                    dac.Connection = ConnectionType.ODBC;
                    dac.DataSource = dsn;
                    dac.Auth = GetAuthTypeOfDsn(dsnKey);
                    dac.Username = GetUsernameOfDsn(dsnKey);
                    dac.Password = GetPasswordOfDsn(dsn);
                    _odbcConnections.Add(dac);
                }
            }
            catch
            {
            }
        }

        /// <summary>Looks in the registry entry for a DSN to find the saved authentication mode</summary>
        /// <param name="key">Registry key for the DSN connection</param>
        /// <returns></returns>
        private AuthType GetAuthTypeOfDsn(RegistryKey key)
        {
            try
            {
                string[] values = key.GetValueNames();
                AuthType auth;
                auth = (values.Contains("Trusted_Connection") && key.GetValue("Trusted_Connection").ToString() == "Yes") ? AuthType.Integrated : AuthType.Basic;
                return auth;
            }
            catch
            {
                return AuthType.Basic;
            }
        }

        /// <summary>Looks in the registry entry for a DSN to find the saved username</summary>
        /// <param name="key">Registry key for the DSN connection</param>
        /// <returns>String representing the username of the DSN</returns>
        private string GetUsernameOfDsn(RegistryKey key)
        {
            try
            {
                string[] values = key.GetValueNames();
                string name;
                name = (values.Contains("LastUser")) ? key.GetValue("LastUser").ToString() : null;
                return name;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Looks in recent connections to see if the selected ODBC has a saved password</summary>
        /// <param name="DataSource">Name of the DSN</param>
        /// <returns>String representing the saved password</returns>
        private string GetPasswordOfDsn(string DataSource)
        {
            DataAccessConnection dac = _recentConnections.FirstOrDefault(c => c.Connection == ConnectionType.ODBC && c.DataSource == DataSource);
            string pass = null;
            if (dac != null)
            {
                pass = dac.Password;
            }
            return pass;
        }

        #endregion

        #region Private Variables and Methods - Encryption

        /// <summary>Encrypts a given string for storage</summary>
        /// <param name="input">SecureString containing the value to encrypt</param>
        /// <returns>String representing the encrypted value</returns>
        public string EncryptString(System.Security.SecureString input)
        {
            byte[] encryptedData = ProtectedData.Protect(
                System.Text.Encoding.Unicode.GetBytes(ToInsecureString(input)),
                entropy,
                DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>Decrypts an encrypted string from storage</summary>
        /// <param name="encryptedData">Encrypted string to decrypt</param>
        /// <returns>SecureString containing the decrypted value</returns>
        public SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    entropy,
                    DataProtectionScope.CurrentUser);
                return ToSecureString(System.Text.Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        /// <summary>Converts a string to a SecureString</summary>
        /// <param name="input">Value to convert</param>
        /// <returns>SecureString object with the same value as the input</returns>
        public SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        /// <summary>Converts a SecureString to a string</summary>
        /// <param name="input">SecureString to convert</param>
        /// <returns>String with the same value as the input</returns>
        public string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }

        #endregion
    }
}
