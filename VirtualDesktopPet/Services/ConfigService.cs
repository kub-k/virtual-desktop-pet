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

        public void SaveConfig(PetConfig config)
        {
            try
            {
                string configDir = Path.GetDirectoryName(configPath);
                if (!Directory.Exists(configDir))
                {
                    Directory.CreateDirectory(configDir);
                }

                string jsonContent = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(configPath, jsonContent);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving config: {ex.Message}", ex);
            }
        }
    }
}