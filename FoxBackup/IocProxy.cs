namespace FoxBackup;

internal static class IocProxy
{
    public static object Get(Type serviceType) => Worker.Container.GetInstance(serviceType);

    public static TService Get<TService>()
        where TService : class
    {
        return Worker.Container.GetInstance<TService>();
    }
}
