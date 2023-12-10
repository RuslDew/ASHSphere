using UnityEngine;
using UnityEngine.UI;

public class EnableOrDisableButtons : MonoBehaviour
{
    public void EnableAllButtons()
    {
        Button[] allButtons = GameObject.FindObjectsByType<Button>(sortMode: FindObjectsSortMode.None);

        foreach (Button button in allButtons)
            button.enabled = true;
    }

    public void DisableAllButtons()
    {
        Button[] allButtons = GameObject.FindObjectsByType<Button>(sortMode: FindObjectsSortMode.None);

        foreach (Button button in allButtons)
            button.enabled = false;
    }
}
