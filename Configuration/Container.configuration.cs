using System;
using MAS.Payments.DataBase.Access;
using MAS.Payments.Infrastructure;
using MAS.Payments.Infrastructure.Command;
using MAS.Payments.Infrastructure.MailMessaging;
using MAS.Payments.Infrastructure.Query;
using MAS.Payments.Notifications;
using SimpleInjector;

namespace MAS.Payments.Configuration
{
    public static class ContainerConfiguration
    {
        //// <para>
        //// Custom dependency container configuration
        //// </para>
        public static void Configure(this Container container)
        {
            container.Register(
                typeof(IQueryHandler<,>),
                typeof(IQueryHandler<,>).Assembly);

            // container.Register(
            //     typeof(INotificator),
            //     typeof(INotificator).Assembly);

            container.Register(
                typeof(ICommandHandler<>),
                AppDomain.CurrentDomain.GetAssemblies());

            container.RegisterDecorator(
                typeof(ICommandHandler<>),
                typeof(TransactionCommandHandlerDecorator<>));

            container.Register(typeof(IResolver), typeof(Resolver), Lifestyle.Singleton);
            container.Register(typeof(IQueryProcessor), typeof(QueryProcessor), Lifestyle.Singleton);
            container.Register(typeof(ICommandProcessor), typeof(CommandProcessor), Lifestyle.Singleton);
            container.Register(typeof(INotificationProcessor), typeof(NotificationProcessor), Lifestyle.Singleton);
            container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);
            container.Register(typeof(IRepository<>), typeof(Repository<>));
            container.Register<IMailSender, MailSender>();
            container.Register<ISmtpClientFactory, SmtpClientFactory>();
            container.Register<IMailBuilder, MailBuilder>();
        }
    }
}