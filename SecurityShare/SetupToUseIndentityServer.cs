using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class SetupToUseIndentityServer
    {
        public static void SetupWebApiUseIdentityServer(IServiceCollection services, IConfiguration configuration)
        {
            SetupWebApiUseIdentityServer(services, GetIdentitySettings(configuration));
        }

        public static IdentitySetting GetIdentitySettings(IConfiguration configuration)
        {
            IdentitySetting identitySetting = new IdentitySetting();
            configuration.GetSection("IdentitySetting").Bind(identitySetting);
            string envClientId = Environment.GetEnvironmentVariable("CLIENTID");
            if (!string.IsNullOrEmpty(envClientId))
                identitySetting.ClientId = envClientId;

            string envClientSecrets = Environment.GetEnvironmentVariable("CLIENTSECRETS");
            if (!string.IsNullOrEmpty(envClientSecrets))
                identitySetting.ClientSecrets = envClientSecrets;

            string envIdentityServerUrl = Environment.GetEnvironmentVariable("IDENTITYSERVERURL");
            if (!string.IsNullOrEmpty(envIdentityServerUrl))
                identitySetting.IdentityServerUrl = envIdentityServerUrl;

            string apiSecret = Environment.GetEnvironmentVariable("APISECRET");
            if (!string.IsNullOrEmpty(apiSecret))
                identitySetting.ApiSecret = apiSecret;

            string ApiName = Environment.GetEnvironmentVariable("APINAME");
            if (!string.IsNullOrEmpty(ApiName))
                identitySetting.ApiName = ApiName;


            return identitySetting;
        }

        public static void SetupWebApiUseIdentityServer(IServiceCollection services, IdentitySetting identitySetting)
        {
            services.AddAuthentication(identitySetting.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = identitySetting.IdentityServerUrl; // 
                    options.RequireHttpsMetadata = identitySetting.RequireHttpsMetadata;

                    options.ApiName = identitySetting.ApiName;
                    options.ApiSecret = identitySetting.ApiSecret;
                });
        }
    }
}
