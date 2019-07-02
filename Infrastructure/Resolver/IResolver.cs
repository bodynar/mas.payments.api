using System;

namespace MAS.Payments.Infrastructure
{
    public interface IResolver
    {
        TService Resolve<TService>()
            where TService : class;

        object GetInstance(Type serviceType);
    }
}
