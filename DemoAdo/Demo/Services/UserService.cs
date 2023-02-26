using Demo.Entities;
using Demo.Services;
using DemoAdo.Data;
using DemoAdo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAdo.Services
{
    public class UserService: IUserService
    {
        private readonly IDataAccess<UserTest> _dataAccess;
        public UserService(IDataAccess<UserTest> dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }
        public List<UserTest> GetAll()
        {
            return _dataAccess.GetAll();
        }
        public void Add(UserTest t)
        {
            _dataAccess.Add(t);

        }
        public void Delete(int Id)
        {
            _dataAccess.Delete(Id);

        }

        public List<UserTest> GetAllWithJoin()
        {
            StringBuilder join = new StringBuilder();
            var className = typeof(UserTest).ToString().Split('.');
            var classNameOrder = typeof(Order).ToString().Split('.');
            join.Append($"inner join {classNameOrder[classNameOrder.Length - 1]} on {className[className.Length - 1]}.{classNameOrder[classNameOrder.Length - 1]}Id = {classNameOrder[classNameOrder.Length - 1]}.Id");
            return _dataAccess.GetAll(join);
        }
    }
}
