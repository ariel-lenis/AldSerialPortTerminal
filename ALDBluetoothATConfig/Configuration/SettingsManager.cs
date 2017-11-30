using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ALDBluetoothATConfig.Configuration
{
    public class SettingsManager
    {
        public ApplicationSettings CurrentSettings { get; set; }

        string fileName;

        public SettingsManager(string fileName)
        {
            this.fileName = fileName;
            this.CurrentSettings = new ApplicationSettings();
        }

        public void LoadFromFile()
        {
            string json;
            if (File.Exists(this.fileName))
            {
                json = File.ReadAllText(this.fileName);
                this.CurrentSettings = JsonConvert.DeserializeObject<ApplicationSettings>(json);
            }
        }

        public void SaveToFile()
        {
            string json = JsonConvert.SerializeObject(this.CurrentSettings, Formatting.Indented);
            File.WriteAllText(this.fileName, json);
        }
    }
}
