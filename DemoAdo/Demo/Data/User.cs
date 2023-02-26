using DemoAdo.Data;
using DemoAdo.Entities;

namespace Demo.Data
{
    public class User : DataAccess<UserTest>, IDataAccess<UserTest>
    {
        public User(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
