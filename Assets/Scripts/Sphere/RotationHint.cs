using UnityEngine;

public class RotationHint : MonoBehaviour
{
    [SerializeField] private GroupsController _groupsController;
    [SerializeField] private Transform _hintArrowsVisual;

    private PiecesGroup _currentlyHighlitedGroup;

    private bool _isHintAlreadyShown = false;


    private void Awake()
    {
        _groupsController.OnAddActionToHistory += HideHint;
    }

    public void ShowHint()
    {
        if (_isHintAlreadyShown)
            return;

        HistoryAction lastHistoryAction = _groupsController.GetLastHistoryAction();

        if (lastHistoryAction == null)
            return;

        PiecesGroup actionGroup = lastHistoryAction.RotatedGroup;

        Vector3 groupCenter = actionGroup.GetActualCenterPosition();
        Vector3 groupAxis = actionGroup.Axis;

        _hintArrowsVisual.position = groupCenter;
        _hintArrowsVisual.up = groupAxis;
        _hintArrowsVisual.position += _hintArrowsVisual.up * actionGroup.OffsetForHint;

        actionGroup.EnableHighlight();

        _hintArrowsVisual.gameObject.SetActive(true);

        _currentlyHighlitedGroup = actionGroup;

        _isHintAlreadyShown = true;
    }

    public void HideHint()
    {
        if (_hintArrowsVisual.gameObject.activeSelf)
        {
            _hintArrowsVisual.gameObject.SetActive(false);

            if (_currentlyHighlitedGroup != null)
                _currentlyHighlitedGroup.DisableHighlight();
        }

        _isHintAlreadyShown = false;
    }
}
