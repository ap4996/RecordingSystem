using UnityEngine;
using Zenject;

public class RecordingSystem : MonoBehaviour
{
    public double timestamp;

    private bool isRecording;

    private RecordingData recordingData;

    private SignalBus _signalBus;

    [Inject]
    private void InjectSignalBus(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<StartRecording>(StartRecording);
        _signalBus.Subscribe<StopRecording>(StopRecording);
        _signalBus.Subscribe<RecordDragging>(RecordDragging);
    }

    private void Update()
    {
        if(isRecording)
        {
            timestamp += Time.deltaTime;
        }
    }

    private void StartRecording()
    {
        isRecording = true;
        recordingData = new RecordingData();
    }

    private void StopRecording()
    {
        isRecording = false;
        timestamp = 0f;
        string jsonString = JsonUtility.ToJson(recordingData);
        Debug.Log(jsonString);
        Debug.Log($"Object {recordingData.records.Count}");
        RecordingData rd = JsonUtility.FromJson<RecordingData>(jsonString);
        Debug.Log($"Object from json {rd.records[0].buttonAnchoredPosition}");
    }

    private void RecordDragging(RecordDragging signal)
    {
        recordingData.records.Add(new Record { timestamp = timestamp, buttonAnchoredPosition = signal.anchoredPosition, buttonName = signal.buttonName });
        Debug.Log($"Record added {recordingData.records.Count}");
    }
}
