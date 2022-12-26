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
}

public struct RecordDragging
{
    public Vector2 anchoredPosition;
    public string buttonName;
}

public struct StartRecording
{
}

public struct StopRecording
{
}