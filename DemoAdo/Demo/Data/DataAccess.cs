using DemoAdo.Data;
using DemoAdo.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAdo.Data
{
    public class DataAccess<T> : IDataAccess<T> where T : class, new()
    {
        private readonly IDbConnection dbConnection;
        private readonly IConfiguration _configuration;
        public DataAccess(IConfiguration configuration)
        {
            var _configuration = configuration.GetSection("SqlConnect:ConnectString").Value;
            dbConnection = new SqlConnection(_configuration);
        }

        public List<T> GetAll()
        {
            List<T> users = new List<T>();
            
            dbConnection.Open();
            var className = typeof(T).ToString().Split('.');
            string sqlString = $"Select * from {className[className.Length - 1]}";
            using IDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = sqlString;
            IDataReader dataReader = dbCommand.ExecuteReader();
            while (dataReader.Read())
            {
                T t = new T();
                foreach (var property in typeof(T).GetProperties())
                {
                    property.SetValue(t, dataReader[property.Name]);
                }
                users.Add(t);
            }
            return users;
        }

        public void Add(T t)
        {
            dbConnection.Open();
            using IDbCommand dbCommand = dbConnection.CreateCommand();
            IDbTransaction transaction = dbConnection.BeginTransaction();
            try
            {
                
                StringBuilder sqlColumn = new StringBuilder();
                StringBuilder sqlColumnValue = new StringBuilder();
                foreach (var userProperty in t.GetType().GetProperties())
                {
                    if (userProperty.Name == "Id")
                        continue;

                    IDbDataParameter dbDataParameter = dbCommand.CreateParameter();
                    dbDataParameter.Value = userProperty.GetValue(t);
                    if (userProperty.PropertyType == typeof(int) || userProperty.PropertyType == typeof(int?))
                    {
                        dbDataParameter.DbType = System.Data.DbType.Int32;
                    }
                    else if (userProperty.PropertyType == typeof(long) || userProperty.PropertyType == typeof(long?))
                    {
                        dbDataParameter.DbType = System.Data.DbType.Int64;
                    }
                    else if (userProperty.PropertyType == typeof(bool))
                    {
                        dbDataParameter.DbType = System.Data.DbType.Int32;
                        dbDataParameter.Value = userProperty.GetValue(t) != null && (bool)userProperty.GetValue(t) ? 1 : 0;
                    }
                    else if (userProperty.PropertyType == typeof(byte[]))
                    {
                        dbDataParameter.DbType = System.Data.DbType.Binary;
                    }
                    else if (userProperty.PropertyType == typeof(DateTime) || userProperty.PropertyType == typeof(DateTime?))
                    {
                        if (userProperty.GetValue(t) != null && ((DateTime?)userProperty.GetValue(t)).HasValue)
                        {
                            dbDataParameter.Value = ((DateTime)userProperty.GetValue(t)).ToString("yyyy-MM-dd HH:mm:ss.ffffff");
                        }
                        dbDataParameter.DbType = System.Data.DbType.String;
                    }
                    else
                    {
                        dbDataParameter.DbType = System.Data.DbType.String;
                    }
                    sqlColumn.Append(userProperty.Name);
                    sqlColumn.Append(",");
                    sqlColumnValue.Append("@" + userProperty.Name + ",");
                    dbDataParameter.ParameterName = "@" + userProperty.Name;
                    dbCommand.Parameters.Add(dbDataParameter);
                }
                var className = typeof(T).ToString().Split('.');
                dbCommand.CommandText = $"Insert into {className[className.Length - 1]}({sqlColumn.ToString().TrimEnd(',')}) values({sqlColumnValue.ToString().TrimEnd(',')})"; 
                dbCommand.Transaction = transaction;
                dbCommand.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            finally{

                dbConnection.Close();
            }
        }

        public void Delete(int Id)
        {
            dbConnection.Open();
           
            using IDbCommand dbCommand = dbConnection.CreateCommand();

            StringBuilder sqlColumn = new StringBuilder();
            StringBuilder sqlColumnValue = new StringBuilder();

            IDbDataParameter dbDataParameter = dbCommand.CreateParameter();
            dbDataParameter.DbType = DbType.Int32;

            dbDataParameter.ParameterName = "@Id";
            dbDataParameter.Value = Id;
            dbCommand.Parameters.Add(dbDataParameter);
            var className = typeof(T).ToString().Split('.');
            dbCommand.CommandText = $"Delete from {className[className.Length - 1]} where Id = @Id"; 
            dbCommand.ExecuteNonQuery();
            dbConnection.Close();
        }

        public List<T> GetAll(StringBuilder join)
        {
            List<T> users = new List<T>();

            dbConnection.Open();
            var className = typeof(T).ToString().Split('.');
            string sqlString = $"Select * from {className[className.Length - 1]}";
            sqlString += join;
            using IDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = sqlString;
            IDataReader dataReader = dbCommand.ExecuteReader();
            while (dataReader.Read())
            {
                T t = new T();
                foreach (var property in typeof(T).GetProperties())
                {
                    property.SetValue(t, dataReader[property.Name]);
                }
                users.Add(t);
            }
            return users;
        }
    }
}
