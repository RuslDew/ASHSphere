using System;
using UnityEngine;
using TMPro;

public class InputNameScreen : MenuScreen
{
    [SerializeField] private TMP_InputField _inputField;

    [SerializeField] private GameController _gameController;

    private bool _mixColors = false;


    public void SetMixMode(bool mixColors)
    {
        _mixColors = mixColors;
    }

    public override void Show(Action onComplete = null)
    {
        _inputField.text = "";

        base.Show(onComplete);
    }

    public void ApplyNameHandler()
    {
        string sessionName = _inputField.text;

        if (_mixColors)
            _gameController.StartGameMixed(sessionName);
        else
            _gameController.StartGameAssembled(sessionName);
    }
}
