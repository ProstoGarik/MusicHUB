using HubClassLibrary;
using System.Text.Json;

public class FileManager
{
    private readonly string tempFolderPath = "/app/data"; // Используем папку в контейнере

    public void SaveFile(TrackList trackList)
    {
        var options = new JsonSerializerOptions { WriteIndented = false };

        // Убедимся, что папка существует
        if (!Directory.Exists(tempFolderPath))
            Directory.CreateDirectory(tempFolderPath);

        string jsonString = JsonSerializer.Serialize(trackList, options);
        File.WriteAllText(Path.Combine(tempFolderPath, "tracklist.json"), jsonString);
    }

    public TrackList LoadFile()
    {
        try
        {
            string jsonFile = File.ReadAllText(Path.Combine(tempFolderPath, "tracklist.json"));
            return JsonSerializer.Deserialize<TrackList>(jsonFile);
        }
        catch
        {
            return new TrackList();
        }
    }
}
