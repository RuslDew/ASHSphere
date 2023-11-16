using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : ScreenBase
{
    [SerializeField] private VerticalLayoutGroup _layout;


    public override void Show()
    {
        _layout.enabled = false;

        base.Show();
    }

    public override void Hide()
    {
        _layout.enabled = false;

        base.Hide();
    }
}
