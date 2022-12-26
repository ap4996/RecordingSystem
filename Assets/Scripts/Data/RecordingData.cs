using System;
using UnityEngine;

[Serializable]
public class RecordingData 
{
    public double timestamp;
    public Vector2 buttonAnchoredPosition;
    public string buttonName;
}

public struct RecordDragging
{
    public Vector2 anchoredPosition;
    public string buttonName;
}