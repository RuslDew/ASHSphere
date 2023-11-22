using System;
using System.Collections.Generic;
using UnityEngine;

public class SavesScreen : MenuScreen
{
    [SerializeField] private SaveButton _saveButtonPrefab;

    private List<SaveButton> _spawnedButtons = new List<SaveButton>();

    [SerializeField] private Transform _buttonsSpawnParent;

    public Action<SaveData> OnSelectSave;
    public Action<SaveData> OnRemoveSave;

    private SaveManager _saveManager;

    [Space]

    [SerializeField] private GameObject _emptySavesText;


    public void Init(SaveManager saveManager)
    {
        _saveManager = saveManager;
    }

    public override void Show(Action onComplete = null)
    {
        RespawnButtons();

        base.Show(onComplete);
    }

    private void RespawnButtons()
    {
        foreach (SaveButton button in _spawnedButtons)
        {
            Destroy(button.gameObject);
        }

        _spawnedButtons.Clear();

        List<SaveData> saveDatas = _saveManager.GetSaveDatas();

        _emptySavesText.SetActive(saveDatas.Count == 0);

        foreach (SaveData data in saveDatas)
        {
            SaveButton button = Instantiate(_saveButtonPrefab, _buttonsSpawnParent);
            button.Init(data);
            button.OnClickSelect += () => SelectSaveHandler(button);
            button.OnClickRemove += () => RemoveSaveHandler(button);

            _spawnedButtons.Add(button);
        }
    }

    private void SelectSaveHandler(SaveButton button)
    {
        OnSelectSave?.Invoke(button.Data);
    }

    private void RemoveSaveHandler(SaveButton button)
    {
        _spawnedButtons.Remove(button);

        OnRemoveSave?.Invoke(button.Data);

        Destroy(button.gameObject);

        _emptySavesText.SetActive(_spawnedButtons.Count == 0);
    }
}
