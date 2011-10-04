using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SQLiteBrowser.DataAccess
{
    public class DataAccessBase
    {
        private IDbConnection _conn;
        private IDbCommand _comm;
        private IDbTransaction _tran;

        public DataAccessBase()
        {
        }

        public string ConnectionString
        {
            get;
            set;
        }

        public IDataReader ExecuteReader(string sqlQuery)
        {
            PrepareConnection();
            _comm.CommandText = sqlQuery;
            IDataReader reader = _comm.ExecuteReader();
            return reader;
        }

        public int ExecuteNonQuery(string sqlQuery)
        {
            PrepareConnection();
            _comm.CommandText = sqlQuery;
            return _comm.ExecuteNonQuery();
        }

        public void BeginTransaction()
        {
            PrepareConnection();
            _tran = _conn.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_tran != null)
            {
                _tran.Commit();
            }
            if (_conn.State == ConnectionState.Open)
            {
                _conn.Close();
            }
        }

        private void PrepareConnection()
        {
            if (_conn == null)
            {
                _conn = GetDbConnection();
            }

            if (_conn.State == ConnectionState.Closed)
            {
                _conn.Open();
            }

            if (_comm == null)
            {
                _comm = GetDbCommand();
                _comm.Connection = _conn;
            }
        }

        internal abstract IDbConnection GetDbConnection();
        internal abstract IDbCommand GetDbCommand();
    }
}
