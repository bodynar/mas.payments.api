using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace MAS.Payments.Infrastructure.MailMessaging
{
    public class MailBuilder<TMailMessage> : IMailMessageBuilder<TMailMessage>
        where TMailMessage : IMailMessage
    {
        protected SmtpSettings SmtpSettings { get; }

        protected IHostingEnvironment HostingEnvironment { get; }

        protected IResolver Resolver { get; }

        protected string From { get; }

        protected string FolderPath { get; }

        public MailBuilder(
            IResolver resolver
        )
        {
            Resolver = resolver;
            SmtpSettings = resolver.Resolve<IOptions<SmtpSettings>>().Value;
            HostingEnvironment = resolver.Resolve<IHostingEnvironment>();

            From = SmtpSettings.From;
            FolderPath = $"{HostingEnvironment.WebRootPath}/mailTemplates";
        }

        private string GetMessageWrapper(string messageContent)
        {
            var messageWrapper = string.Empty;

            using (var reader = File.OpenText($"{FolderPath}/MailMessageBase.html"))
            {
                messageWrapper = reader.ReadToEnd();
            }

            return messageWrapper.Replace("{{ body }}", messageContent).Replace("{{ BaseCurrentYear }}", DateTime.Now.Year.ToString());
        }

        private string GetMessageTemplate(string templateName, object model = null)
        {
            var messageTemplate = string.Empty;

            using (var reader = File.OpenText($"{FolderPath}/{templateName}MailMessage.html"))
            {
                messageTemplate = reader.ReadToEnd();
            }

            if (model != null)
            {
                messageTemplate = InsertModelData(messageTemplate, model);
            }

            return messageTemplate;
        }

        private string InsertModelData(string messageTemplate, object model)
        {
            var properties = model.GetType().GetProperties().Where(x => new[] {
                    typeof(string),
                    typeof(char),
                    typeof(byte),
                    typeof(sbyte),
                    typeof(ushort),
                    typeof(short),
                    typeof(uint),
                    typeof(int),
                    typeof(ulong),
                    typeof(long),
                    typeof(float),
                    typeof(double),
                    typeof(decimal),
                    typeof(DateTime)
                }.Contains(x.PropertyType));

            foreach (var property in properties)
            {
                if (messageTemplate.Contains($"{{ {property.Name} }}"))
                {
                    messageTemplate = messageTemplate.Replace("{{ " + property.Name + " }}", property.GetValue(model).ToString());
                }
            }

            return messageTemplate;
        }

        public MailMessage Build(TMailMessage mailMessage, object model = null)
        {
            var messageText = GetMessageTemplate(mailMessage.TemplateName, model);
            var body = GetMessageWrapper(messageText);

            return
                new MailMessage(From, mailMessage.Recipient)
                {
                    Body = body,
                    Subject = $"[fi.Sz] Payment service - {mailMessage.Subject}",
                    IsBodyHtml = true
                };
        }
    }
}