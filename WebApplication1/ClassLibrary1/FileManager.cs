using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Text.Json;
using System.IO.Compression;

namespace HubClassLibrary
{

    public class FileManager
    {
        private readonly string tempFolderPath = "F:\\TempFiles";

        public void SaveFile(TrackList trackList)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = false // Отключаем форматирование
            };
            string jsonString = JsonSerializer.Serialize(trackList, options);

            // Записываем JSON в файл
            File.WriteAllText(Path.Combine(tempFolderPath, "tracklist.json"), jsonString);
        }

        public TrackList LoadFile()
        {
            try
            {
                // Читаем JSON из файла
                string jsonFile = File.ReadAllText(Path.Combine(tempFolderPath, "tracklist.json"));

                return JsonSerializer.Deserialize<TrackList>(jsonFile);
            }
            catch
            {
                return new TrackList();
            }
        }
    }
}
