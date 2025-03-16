namespace MAS.Payments.Configuration
{
    using System;

    using MAS.Payments.DataBase.Access;
    using MAS.Payments.Infrastructure;
    using MAS.Payments.Infrastructure.Command;
    using MAS.Payments.Infrastructure.Query;
    using MAS.Payments.Notifications;

    using SimpleInjector;

    public static class ContainerConfiguration
    {
        /// <summary>
        /// Custom dependency container configuration
        /// </summary>
        public static Container Configure(this Container container)
        {
            #region CQRS

            container.Register(
                typeof(IQueryHandler<,>),
                typeof(IQueryHandler<,>).Assembly);

            container.Register(
                typeof(ICommandHandler<>),
                AppDomain.CurrentDomain.GetAssemblies());

            container.RegisterDecorator(
                typeof(ICommandHandler<>),
                typeof(TransactionCommandHandlerDecorator<>));

            container.Register<IQueryProcessor, QueryProcessor>(Lifestyle.Singleton);
            container.Register<ICommandProcessor, CommandProcessor>(Lifestyle.Singleton);

            #endregion

            #region Database

            container.Register(typeof(IRepository<>), typeof(Repository<>));

            #endregion

            #region Notifications

            container.Register<INotificationProcessor, NotificationProcessor>(Lifestyle.Singleton);

            container.Collection.Register<INotificator>(typeof(INotificator).Assembly);

            #endregion

            container.Register<IResolver, Resolver>(Lifestyle.Singleton);

            container.Options.ResolveUnregisteredConcreteTypes = true;

            return container;
        }
    }
}