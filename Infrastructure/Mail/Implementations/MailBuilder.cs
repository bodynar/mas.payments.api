using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace MAS.Payments.Infrastructure.MailMessaging
{
    public class MailBuilder : IMailBuilder
    {
        private string From { get; }

        private string FolderPath { get; }

        public MailBuilder(
            IOptions<SmtpSettings> smtpSettings,
            IHostingEnvironment hostingEnvironment
        )
        {
            From = smtpSettings.Value.From;
            FolderPath = $"{hostingEnvironment.WebRootPath}/mailTemplates";
        }

        public MailMessage FormTestMailMessage(string to)
        {
            var messageText = GetMessageTemplate("Test");
            var body = GetMessageWrapper(messageText);

            var message = new MailMessage(From, to)
            {
                Body = body,
                Subject = "Test mail message",
                IsBodyHtml = true
            };

            return message;
        }

        private string GetMessageWrapper(string messageContent)
        {
            var messageWrapper = string.Empty;

            using (var reader = File.OpenText($"{FolderPath}/MailMessageBase.html"))
            {
                messageWrapper = reader.ReadToEnd();
            }

            return messageWrapper.Replace("{{ body }}", messageContent);
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
                    messageTemplate = messageTemplate.Replace($"{{ {property.Name} }}", property.GetValue(model).ToString());
                }
            }

            return messageTemplate;
        }
    }
}