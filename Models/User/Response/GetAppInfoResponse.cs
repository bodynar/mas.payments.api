namespace MAS.Payments.Models
{
    using System;

    public class GetAppInfoResponse
    {
        public string DataBaseName { get; }

        public string ServerAppVersion { get; }

        public GetAppInfoResponse(string dataBaseName, string serverAppVersion)
        {
            DataBaseName = dataBaseName ?? throw new ArgumentNullException(nameof(dataBaseName));
            ServerAppVersion = serverAppVersion ?? throw new ArgumentNullException(nameof(serverAppVersion));
        }
    }
}