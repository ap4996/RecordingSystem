using System.IO;
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
        _signalBus.Subscribe<RecordInputSignal>(RecordInput);
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
        _signalBus.Fire(new RecordInitialStateOfButtons());
    }

    private void StopRecording(StopRecording signal)
    {
        isRecording = false;
        timestamp = 0f;
        string jsonString = JsonUtility.ToJson(recordingData);
        string path = Application.persistentDataPath + "/" + signal.fileName + ".json";
        File.WriteAllText(path, jsonString);
        Debug.Log($"written in file {path}");
    }

    private void RecordInput(RecordInputSignal signal)
    {
        if(isRecording)
        {
            recordingData.records.Add(new Record { timestamp = timestamp, buttonAnchoredPosition = signal.anchoredPosition, buttonName = signal.buttonName, inputType = signal.inputType });
        }
    }
}
