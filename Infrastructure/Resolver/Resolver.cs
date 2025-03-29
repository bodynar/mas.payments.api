namespace MAS.Payments.Infrastructure
{
    using System;

    using SimpleInjector;

    internal class Resolver(
        Container container
    ) : IResolver
    {
        public TService Resolve<TService>()
            where TService : class
        {
            return container.GetInstance<TService>();
        }

        public object GetInstance(Type serviceType)
        {
            return container.GetInstance(serviceType);
        }
    }
}
