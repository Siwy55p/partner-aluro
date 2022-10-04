namespace partner_aluro.Services.Interfaces
{
    public interface ISessionMenager
    {
        T Get<T>(string key);
        void Set<T>(string name, T value);
        void Abandon();
        T TryGet<T>(string key);

    }
}
