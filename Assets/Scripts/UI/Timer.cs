using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private string _timeFormat = "0:dd\\.hh\\:mm\\:ss";

    private Sequence _updateTimerSequence;


    public void StartTimer(Session session)
    {
        StopTimer();

        _updateTimerSequence = DOTween.Sequence().AppendCallback(() =>
        {
            TimeSpan span = TimeSpan.FromSeconds(session.GameDuration);

            _timerText.text = span.ToString(_timeFormat);

            session.GameDuration++;
        }).AppendInterval(1f).SetLoops(-1);
    }

    public void StopTimer()
    {
        if (_updateTimerSequence != null)
            _updateTimerSequence.Kill();
    }
}
