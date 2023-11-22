using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private ScreenBase _menuScreen;
    [SerializeField] private ScreenBase _inputNameScreen;
    [SerializeField] private ScreenBase _gameScreen;
    [SerializeField] private SavesScreen _savesScreen;
    [SerializeField] private FocusOnSphereAnimation _focusAnim;

    private bool _isGameStarted = false;

    [SerializeField] private GroupsController _groupsController;
    [SerializeField] private Timer _timer;
    [SerializeField] private SaveManager _saveManager;

    private Session _currentSession;


    private void Awake()
    {
        _groupsController.AllowActions(false);

        _savesScreen.OnSelectSave += StartSavedGame;
    }

    public void StartGameMixed(string sessionName)
    {
        if (!_isGameStarted)
        {
            StartNewSession(sessionName);

            _menuScreen.Hide();
            _inputNameScreen.Hide();

            _isGameStarted = true;

            _focusAnim.Focus();
            _groupsController.Mix(() =>
            {
                _groupsController.AllowActions(true);
                _gameScreen.Show();
                _timer.StartTimer(_currentSession);
            });
        }
    }

    public void StartGameAssembled(string sessionName)
    {
        if (!_isGameStarted)
        {
            StartNewSession(sessionName);

            _menuScreen.Hide();
            _inputNameScreen.Hide();

            _isGameStarted = true;

            _focusAnim.Focus(() =>
            {
                _groupsController.AllowActions(true);
                _gameScreen.Show();
                _timer.StartTimer(_currentSession);
            });
        }
    }

    public void StartSavedGame(SaveData data)
    {
        if (!_isGameStarted)
        {
            LoadSession(data);

            _menuScreen.Hide();
            _savesScreen.Hide();

            _isGameStarted = true;

            _focusAnim.Focus();
            _groupsController.LoadHistory(_currentSession.History, () =>
            {
                _groupsController.AllowActions(true);
                _gameScreen.Show();
                _timer.StartTimer(_currentSession);
            });
        }
    }

    public void StopGame()
    {
        if (_isGameStarted)
        {
            _gameScreen.Hide();
            _focusAnim.Unfocus();

            _isGameStarted = false;

            _groupsController.AllowActions(false);

            _groupsController.Assemble(() => _menuScreen.Show());

            _timer.StopTimer();

            SaveCurrentSession();
        }
    }

    private void StartNewSession(string sessionName)
    {
        string correctName = sessionName == "" ? DateTime.Now.ToString() : sessionName;

        _currentSession = new Session(DateTime.Now, correctName);
    }

    private void LoadSession(SaveData sessionData)
    {
        DateTime sessionStartDate = DateTime.Parse(sessionData.StartDate);

        _currentSession = new Session(sessionStartDate, sessionData.SaveName, sessionData.History);
    }

    private void SaveCurrentSession()
    {
        _currentSession.SaveHistory(_groupsController.GetCurrentActionsHistory());

        _saveManager.SaveSession(_currentSession);
    }
}
