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
    public class Service: IService
    {
        private readonly IDataAccess _dataAccess;
        public Service(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public List<UserTest> GetAll()
        {
            return _dataAccess.GetAll();
        }
        public void Add(UserTest userTest)
        {
            _dataAccess.Add(userTest);

        }
        public void Delete(int Id)
        {
            _dataAccess.Delete(Id);

        }
    }
}
