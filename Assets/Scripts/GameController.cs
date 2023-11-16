using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private ScreenBase _menuScreen;
    [SerializeField] private ScreenBase _gameScreen;
    [SerializeField] private FocusOnSphereAnimation _focusAnim;

    private bool _isGameStarted = false;

    [SerializeField] private GroupsController _groupsController;
    [SerializeField] private Timer _timer;

    private Session _currentSession;


    private void Awake()
    {
        _groupsController.AllowActions(false);
    }

    public void StartGameMixed()
    {
        StartGame(true);
    }

    public void StartGameAssembled()
    {
        StartGame(false);
    }

    private void StartGame(bool mixed)
    {
        if (!_isGameStarted)
        {
            _menuScreen.Hide();

            _isGameStarted = true;

            if (mixed)
            {
                _focusAnim.Focus();
                _groupsController.Mix(() =>
                {
                    _groupsController.AllowActions(true);
                    _gameScreen.Show();
                    _timer.StartTimer(_currentSession);
                });
            }
            else
            {
                _focusAnim.Focus(() =>
                {
                    _groupsController.AllowActions(true);
                    _gameScreen.Show();
                    _timer.StartTimer(_currentSession);
                });
            }

            StartNewSession();
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
        }
    }

    public void StartNewSession()
    {
        _currentSession = new Session(System.DateTime.Now);
    }
}
