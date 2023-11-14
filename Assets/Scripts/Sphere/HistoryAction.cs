using System;

[Serializable]
public class HistoryAction
{
    public PiecesGroup RotatedGroup { get; private set; }
    public float AngleChange { get; private set; }


    public HistoryAction(PiecesGroup group, float angleChange)
    {
        RotatedGroup = group;
        AngleChange = angleChange;
    }
}
