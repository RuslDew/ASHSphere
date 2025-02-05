using UnityEngine;
using UnityEngine.Events;
using System;
using System.Runtime.InteropServices;

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

    [Space]

    [SerializeField] private UnityEvent _onCompleteGameStart;
    [SerializeField] private UnityEvent _onCompleteGameStop;

    [DllImport("__Internal")]
    private static extern void UnityPluginRequestJsVKWebAppInit();

    private void Awake()
    {
        UnityPluginRequestJsVKWebAppInit();

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
                _gameScreen.Show(() => _onCompleteGameStart?.Invoke());
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
                _gameScreen.Show(() => _onCompleteGameStart?.Invoke());
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
                _gameScreen.Show(() => _onCompleteGameStart?.Invoke());
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

            SaveCurrentSession();

            _groupsController.Assemble(() => _menuScreen.Show(() => _onCompleteGameStop?.Invoke()));

            _timer.StopTimer();
        }
    }

    private void StartNewSession(string sessionName)
    {
        string correctName = sessionName == "" ? DateTime.Now.ToString() : sessionName;

        _currentSession = new Session(correctName);
    }

    private void LoadSession(SaveData sessionData)
    {
        _currentSession = new Session(sessionData.SaveName, sessionData.History, sessionData.GameDuration);
    }

    private void SaveCurrentSession()
    {
        _currentSession.History = _groupsController.GetCurrentActionsHistory();

        _saveManager.SaveSession(_currentSession);
    }
}
