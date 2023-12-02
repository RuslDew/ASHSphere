using UnityEngine;
using TMPro;

public class RotateAnglePanel : ScreenBase
{
    [SerializeField] private TextMeshProUGUI _angleChangeText;
    [SerializeField] private string _angleChangeTextFormat = "{0}°";


    public void Show(float angleChange)
    {
        _angleChangeText.text = string.Format(_angleChangeTextFormat, Mathf.Round(angleChange));

        Show();
    }
}
