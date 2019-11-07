using System;

using MAS.Payments.DataBase;
using MAS.Payments.Models;

namespace MAS.Payments.Infrastructure.Cache
{
    public static class AuthTokensCache
    {
        public static void SaveOrUpdate(CachedAuthToken cachedToken)
        {
            CacheService.Save($"authToken: {cachedToken.Token}", cachedToken);
        }

        public static void Save(UserToken token)
        {
            var cachedAuthToken = new CachedAuthToken
            {
                Token = token.Token,
                ActiveTo = token.ActiveTo.Value,
                CreatedAt = token.CreatedAt,
                LastChecked = DateTime.UtcNow,
                UserId = token.UserId,
                UserName = token.User.Login
            };

            CacheService.Save($"authToken: {token.Token}", cachedAuthToken);
        }

        public static CachedAuthToken Get(string token)
        {
            return CacheService.Get<CachedAuthToken>($"authToken: {token}");
        }

        public static void Remove(string token)
        {
            CacheService.Remove($"authToken: {token}");
        }
    }
}