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

[Serializable]
public enum InputType
{
    StartingPosition,
    StartingColor,
    Dragging,
    RightClickForColorChange,
    ShowTooltip,
    HideTooltip,
    PopupOpened,
    PopupClosed,
    StopRecording
}

#region signals

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

public struct SetBlockerState
{
    public bool enable;
}

public struct HideTooltipSignal
{

}

public struct ReplayCompleted
{

}

public struct FileDoesNotExistForReplay
{

}

public struct OpenPopup
{
    public string popupContent;
}
#endregion