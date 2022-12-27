using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RecordingData 
{
    public List<Record> records;

    public RecordingData()
    {
        records = new List<Record>();
    }
}

[Serializable]
public class Record
{
    public double timestamp;
    public Vector2 buttonAnchoredPosition;
    public string buttonName;
    public InputType inputType;
    public ColorName colorName;
}

public enum InputType
{
    StartingState,
    Dragging,
    RightClickForColorChange
}

public struct RecordInputSignal
{
    public Vector2 anchoredPosition;
    public string buttonName;
    public ColorName colorName;
    public InputType inputType;
}

public struct RecordInitialStateOfButtons
{

}

public struct StartRecording
{
}

public struct StopRecording
{
    public string fileName;
}

public struct StartReplay
{
    public string fileName;
}

public struct PlayRecording
{
    public Record record;
}