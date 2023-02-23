using DemoAdo.Entities;

namespace DemoAdo.Data
{
    public interface IDataAccess
    {
        List<UserTest> GetAll();
        void Add(UserTest user);
        void Delete(int id);
    }
}
