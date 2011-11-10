﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Xml;
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
                                    if(!string.IsNullOrEmpty(dac.Password))
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
            finally
            {
            }
        }

        private void SaveRecentConnections()
        {
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
                root.Add(xmlConn);
            }

            root.Save(SETTINGS_PATH + RECENT_CONNECTIONS_FILE);
        }

        private XElement CreateElement(string Name, string Value)
        {
            return CreateElement(Name, Value, false);
        }

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

        #region Private Variables and Methods - Encryption

        private byte[] entropy = System.Text.Encoding.ASCII.GetBytes("It was love at first sight. The first time Yossarian saw the chaplain, he fell madly in love with him.");

        public string EncryptString(System.Security.SecureString input)
        {
            byte[] encryptedData = ProtectedData.Protect(
                System.Text.Encoding.Unicode.GetBytes(ToInsecureString(input)),
                entropy,
                DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

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
