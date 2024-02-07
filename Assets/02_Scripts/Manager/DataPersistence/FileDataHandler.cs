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
        if (!File.Exists(fullPath))
            return null;
        try
        {
            var dataToLoad = File.ReadAllText(fullPath);
            return JsonUtility.FromJson<GameData>(dataToLoad);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occured when trying to load data to file: {fullPath} \n" + e);
            return null;
        }
    }

    public void Save(GameData data)
    {
        var fullPath = Path.Combine(_dataDirPath, _dataFileName);
        try
        {
            var directoryPath = Path.GetDirectoryName(fullPath);
            Directory.CreateDirectory(directoryPath!);
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(fullPath,json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occured when trying to save data to file: {fullPath} \n" + e);
        }
    }
}