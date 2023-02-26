using DemoAdo.Entities;
using System.Text;

namespace Demo.Services
{
    public interface IUserService
    {
        List<UserTest> GetAll();
        void Delete(int Id);
        void Add(UserTest user);
        List<UserTest> GetAllWithJoin();
    }
}
