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
    public class DataAccess : IDataAccess
    {
        private readonly IDbConnection dbConnection;
        private readonly IConfiguration _configuration;
        public DataAccess(IConfiguration configuration)
        {
            var _configuration = configuration.GetSection("SqlConnect:ConnectString").Value;
            dbConnection = new SqlConnection(_configuration);
        }

        public List<UserTest> GetAll()
        {
            List<UserTest> users = new List<UserTest>();
            dbConnection.Open();
            string sqlString = $"Select * from UserTest";
            using IDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = sqlString;
            IDataReader dataReader = dbCommand.ExecuteReader();
            while (dataReader.Read())
            {
                users.Add(new UserTest()
                {
                    Id = (int)dataReader["Id"],
                    Name = (string)dataReader["Name"],
                    Address = (string)dataReader["Address"],
                    CreateDate = (DateTime)dataReader["CreateDate"],
                    Deleted = (bool)dataReader["Deleted"],
                    Email = (string)dataReader["Email"],
                    Phone = (string)dataReader["Email"]
                });
            }
            return users;
        }

        public void Add(UserTest userTest)
        {
            dbConnection.Open();
            string sqlString = $"Insert into UserTest(Name, Phone, Email, Address, CreateDate, Deleted) values(@Name, @Phone, @Email, @Address, @CreateDate, @Deleted)";
            using IDbCommand dbCommand = dbConnection.CreateCommand();
            
            StringBuilder sqlColumn = new StringBuilder();
            StringBuilder sqlColumnValue = new StringBuilder();
            foreach (var userProperty in userTest.GetType().GetProperties()) {
                if (userProperty.Name == "Id")
                    continue;
                sqlColumn.Append(userProperty.Name);
                sqlColumn.Append(",");
                sqlColumnValue.Append("@" + userProperty.Name + ",");
          
                IDbDataParameter dbDataParameter = dbCommand.CreateParameter();
                dbDataParameter.DbType = DbType.String;
                
                dbDataParameter.ParameterName = "@" + userProperty.Name;
                dbDataParameter.Value = userProperty.GetValue(userTest);
                dbCommand.Parameters.Add(dbDataParameter);
            }
            dbCommand.CommandText = $"Insert into UserTest({sqlColumn.ToString().TrimEnd(',')}) values({sqlColumnValue.ToString().TrimEnd(',')})"; ;
            dbCommand.ExecuteNonQuery();
            dbConnection.Close();
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
            dbCommand.CommandText = $"Delete from UserTest where Id = @Id"; 
            dbCommand.ExecuteNonQuery();
            dbConnection.Close();
        }
    }
}
