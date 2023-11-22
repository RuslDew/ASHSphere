using UnityEngine;

public class HideAndShowNextScreen : MonoBehaviour
{
    [SerializeField] private ScreenBase _hideScreen;
    [SerializeField] private ScreenBase _showScreen;


    public void ShowNextScreen()
    {
        _hideScreen.Hide(_showScreen.Show);
    }
}
