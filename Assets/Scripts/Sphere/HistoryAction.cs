using System;

[Serializable]
public class HistoryAction
{
    public PiecesGroup RotatedGroup { get; private set; }
    public float PreviousAngle { get; private set; }


    public HistoryAction(PiecesGroup group, float previousAngle)
    {
        RotatedGroup = group;
        PreviousAngle = previousAngle;
    }
}
