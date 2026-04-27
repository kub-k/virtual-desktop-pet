using Newtonsoft.Json;
using System;
using System.IO;
using VirtualDesktopPet.Models;

namespace VirtualDesktopPet.Services
{
    public class ConfigService
    {
        private readonly string configPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "settings.json");

        public PetConfig LoadConfig()
        {
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException(
                    "Settings file was not found.",
                    configPath
                );
            }

            string jsonContent = File.ReadAllText(configPath);

            PetConfig config =
                JsonConvert.DeserializeObject<PetConfig>(jsonContent);

            return config;
        }
    }
}