using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HyperlinksOpener : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _targetText;


    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(_targetText, eventData.position, eventData.pressEventCamera);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = _targetText.textInfo.linkInfo[linkIndex];

            Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}
