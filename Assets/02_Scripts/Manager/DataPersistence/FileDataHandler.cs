using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private readonly string _dataDirPath;
    private readonly string _dataFileName;

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        _dataDirPath = dataDirPath;
        _dataFileName = dataFileName;
    }
    public GameData Load()
    {
        var fullPath = Path.Combine(_dataDirPath, _dataFileName);
        GameData loadedData = null;
        if (!File.Exists(fullPath))
            return null;
        try
        {
            var dataToLoad = "";
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    dataToLoad = reader.ReadToEnd();
                }
            }
            loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occured when trying to load data to file: {fullPath} \n" + e);
            return null;
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        var fullPath = Path.Combine(_dataDirPath, _dataFileName);
        try
        {
            var directoryPath = Path.GetDirectoryName(fullPath);
            Directory.CreateDirectory(directoryPath!);
            var dataToStore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occured when trying to save data to file: {fullPath} \n" + e);
        }
    }
}