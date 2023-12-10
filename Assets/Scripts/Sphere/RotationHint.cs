using UnityEngine;
using TMPro;
using DG.Tweening;

public class RotationHint : MonoBehaviour
{
    [SerializeField] private GroupsController _groupsController;
    [SerializeField] private Transform _hintArrowsVisual;

    private PiecesGroup _currentlyHighlitedGroup;

    private bool _isHintAlreadyShown = false;

    [SerializeField] private TextMeshPro _angleText;
    [SerializeField] private TextMeshPro _oppositeAngleText;
    [SerializeField] private float _angleTextOffset = 1f;
    [SerializeField] private string _angleTextFormat = "{0}°";

    private Sequence _updateAngleTextSequence;

    [Space]

    [SerializeField] private RendererTextureOffsetAnimation _animation;

    [SerializeField] private ScaleByScreenSize _sphereScaler;


    private void Start()
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
        float angleChange = lastHistoryAction.AngleChange;

        float groupAxisSum = groupAxis.x + groupAxis.y + groupAxis.z;
        float groupAxisSign = -groupAxisSum.GetSign();

        float rotationMultiplier = (angleChange > 0 ? 1 : -1);
        _animation.SetMultiplier(rotationMultiplier);

        _hintArrowsVisual.position = groupCenter;
        _hintArrowsVisual.up = groupAxis;
        _hintArrowsVisual.position += _hintArrowsVisual.up * actionGroup.OffsetForHint;

        actionGroup.EnableHighlight();

        _hintArrowsVisual.gameObject.SetActive(true);

        _currentlyHighlitedGroup = actionGroup;

        StartUpdateAngleText(angleChange);

        _isHintAlreadyShown = true;
    }

    public void HideHint()
    {
        if (_hintArrowsVisual.gameObject.activeSelf)
        {
            _hintArrowsVisual.gameObject.SetActive(false);

            if (_currentlyHighlitedGroup != null)
                _currentlyHighlitedGroup.DisableHighlight();

            StopUpdateAngleText();
        }

        _isHintAlreadyShown = false;
    }

    private void StartUpdateAngleText(float historyAngleChange)
    {
        StopUpdateAngleText();

        _angleText.text = string.Format(_angleTextFormat, -historyAngleChange);
        _angleText.gameObject.SetActive(true);

        _oppositeAngleText.text = string.Format(_angleTextFormat, -historyAngleChange);
        _oppositeAngleText.gameObject.SetActive(true);

        _updateAngleTextSequence = DOTween.Sequence().AppendCallback(() =>
        {
            Vector3 pos = GetPosForAngleText();
            _angleText.transform.position = pos;
            _oppositeAngleText.transform.position = new Vector3(-pos.x, pos.y, pos.z);

            _angleText.transform.LookAt(Camera.main.transform);
            _oppositeAngleText.transform.LookAt(Camera.main.transform);
        }).SetLoops(-1);
    }

    private void StopUpdateAngleText()
    {
        if (_updateAngleTextSequence != null)
            _updateAngleTextSequence.Kill();

        _angleText.gameObject.SetActive(false);
        _oppositeAngleText.gameObject.SetActive(false);
    }

    private Vector3 GetPosForAngleText()
    {
        Vector3 rightVector = new Vector3(Mathf.Abs(_hintArrowsVisual.right.x) + Mathf.Abs(_hintArrowsVisual.right.z), _hintArrowsVisual.right.y, 0f);
        rightVector.Normalize();

        Vector3 pos = _hintArrowsVisual.position + (rightVector * _angleTextOffset * _sphereScaler.ScaleMultiplier);

        return pos;
    }
}
