using System; 
namespace partner_aluro.Services.Interfaces
{
    public class SessionMenager : ISessionMenager
    {
        private HttpContext session;
        public SessionMenager()
        {

        }
        public void Abandon()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string name, T value)
        {
            throw new NotImplementedException();
        }

        public T TryGet<T>(string key)
        {
            throw new NotImplementedException();
        }
    }
}
