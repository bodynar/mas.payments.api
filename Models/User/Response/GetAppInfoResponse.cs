namespace MAS.Payments.Models
{
    using System;

    public class GetAppInfoResponse(
        string dataBaseName,
        string serverAppVersion
    )
    {
        public string DataBaseName { get; } = dataBaseName ?? throw new ArgumentNullException(nameof(dataBaseName));

        public string ServerAppVersion { get; } = serverAppVersion ?? throw new ArgumentNullException(nameof(serverAppVersion));
    }
}