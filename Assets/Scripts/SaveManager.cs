using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SaveData
{
    public string SaveName { get; private set; }
    public string StartDate { get; private set; }
    public string History { get; private set; }

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

public class SaveManager : MonoBehaviour
{
    [SerializeField] private string _playerPrefsName = "saves";

    private List<SaveData> _saveDatas = new List<SaveData>();


    private void Awake()
    {
        LoadData();
        SpawnButtons();
    }

    private void LoadData()
    {

    }

    private void SpawnButtons()
    {

    }

    public void AddSession(Session session)
    {
        foreach (SaveData data in _saveDatas)
        {
            if (data.SaveName == session.SessionName)
            {
                data.UpdateHistory(session.History);

                return;
            }
        }

        _saveDatas.Add(new SaveData(session.SessionName, session.SessionStartDate.ToString(), session.History));
    }
}
