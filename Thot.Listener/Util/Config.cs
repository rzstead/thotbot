using System;
using Microsoft.Extensions.Configuration;

namespace Thot.Listener.Util
{
    public class Config
    {
        private readonly IConfiguration _config;
        public string DiscordToken => _config["DiscordToken"];
        public Config()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", false, true)
                .Build();
        }
    }
}