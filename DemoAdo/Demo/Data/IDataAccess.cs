using DemoAdo.Entities;
using System.Text;

namespace DemoAdo.Data
{
    public interface IDataAccess<T> where T : class, new()
    {
        public List<T> GetAll();
        public void Add(T user);
        public void Delete(int id);
        public List<T> GetAll(StringBuilder join);
    }
}
