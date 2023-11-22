using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SaveData
{
    public string SaveName;

    public string StartDate;
    public string History;

    public SaveData(string saveName, string startDate, string history)
    {
        SaveName = saveName;
        StartDate = startDate;
        History = history;
    }

    public void UpdateHistory(string newHistory)
    {
        History = newHistory;
    }
}

[Serializable]
public class SaveDataList
{
    public List<SaveData> SaveDatas = new List<SaveData>();
}

[Serializable]
public class SaveManager : MonoBehaviour
{
    [SerializeField] private string _playerPrefsName = "saves";
    [SerializeField] private SavesScreen _savesScreen;

    [SerializeField] private SaveDataList _saveDatas = new SaveDataList();


    private void Awake()
    {
        LoadData();

        _savesScreen.Init(this);

        _savesScreen.OnRemoveSave += RemoveSave;
    }

    private void LoadData()
    {
        _saveDatas.SaveDatas.Clear();

        string dataJson = PlayerPrefs.GetString(_playerPrefsName, "");

        if (dataJson == "")
            return;

        _saveDatas = JsonUtility.FromJson<SaveDataList>(dataJson);

        if (_saveDatas == null)
        {
            _saveDatas = new SaveDataList();
            return;
        }
    }

    private void SaveData()
    {
        string dataJson = JsonUtility.ToJson(_saveDatas);

        PlayerPrefs.SetString(_playerPrefsName, dataJson);
        PlayerPrefs.Save();
    }

    public void SaveSession(Session session)
    {
        foreach (SaveData data in _saveDatas.SaveDatas)
        {
            if (data.SaveName == session.SessionName)
            {
                data.UpdateHistory(session.History);

                return;
            }
        }

        _saveDatas.SaveDatas.Add(new SaveData(session.SessionName, session.SessionStartDate.ToString(), session.History));

        SaveData();
    }

    private void RemoveSave(SaveData removingData)
    {
        foreach (SaveData data in _saveDatas.SaveDatas)
        {
            if (data.SaveName == removingData.SaveName)
            {
                _saveDatas.SaveDatas.Remove(data);

                SaveData();

                return;
            }
        }
    }

    public List<SaveData> GetSaveDatas()
    {
        return _saveDatas.SaveDatas;
    }
}
