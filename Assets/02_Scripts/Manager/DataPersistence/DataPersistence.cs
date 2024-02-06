public interface DataPersistence
{
    void LoadData(GameData data);

    void SaveData(ref GameData data);
}
