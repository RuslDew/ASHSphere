using System;

[Serializable]
public class HistoryAction
{
    public PiecesGroup RotatedGroup { get; private set; }
    public float OldAngle { get; private set; }
    public float NewAngle { get; private set; }

    public HistoryAction(PiecesGroup group, float oldAngle, float newAngle)
    {
        RotatedGroup = group;
        OldAngle = oldAngle;
        NewAngle = newAngle;
    }
}
