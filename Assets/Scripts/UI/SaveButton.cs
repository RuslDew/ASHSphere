using UnityEngine;
using System;
using TMPro;

public class SaveButton : MonoBehaviour
{
    public SaveData Data { get; private set; }

    public Action OnClickSelect; 
    public Action OnClickRemove;

    [SerializeField] private TextMeshProUGUI _saveNameText;


    public void Init(SaveData data)
    {
        Data = data;

        _saveNameText.text = Data.SaveName;
    }

    public void ClickSelectHandler()
    {
        OnClickSelect?.Invoke();
    }

    public void ClickRemoveHandler()
    {
        OnClickRemove?.Invoke();
    }
}
