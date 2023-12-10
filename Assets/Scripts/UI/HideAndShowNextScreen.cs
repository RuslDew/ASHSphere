using UnityEngine;
using UnityEngine.Events;

public class HideAndShowNextScreen : MonoBehaviour
{
    [SerializeField] private ScreenBase _hideScreen;
    [SerializeField] private ScreenBase _showScreen;

    [SerializeField] private UnityEvent OnAnimEnd;


    public void ShowNextScreen()
    {
        _hideScreen.Hide(() => _showScreen.Show(() => OnAnimEnd?.Invoke()));
    }
}
