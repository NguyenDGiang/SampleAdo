using DemoAdo.Entities;

namespace Demo.Services
{
    public interface IService
    {
        List<UserTest> GetAll();
        void Delete(int Id);
        void Add(UserTest user);
    }
}
