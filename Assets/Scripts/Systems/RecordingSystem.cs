using System.IO;
using UnityEngine;
using Zenject;

public class RecordingSystem : MonoBehaviour
{
    public double timestamp;

    private bool isRecording;

    private RecordingData recordingData;

    #region Zenject

    private SignalBus _signalBus;

    [Inject]
    private void InjectSignalBus(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<StartRecording>(StartRecording);
        _signalBus.Subscribe<StopRecording>(StopRecording);
        _signalBus.Subscribe<RecordInputSignal>(RecordInput);
    }

    private void StartRecording()
    {
        isRecording = true;
        recordingData = new RecordingData();
        _signalBus.Fire(new RecordInitialStateOfButtons());
    }

    private void StopRecording(StopRecording signal)
    {
        _signalBus.Fire(new RecordInputSignal
        {
            inputType = InputType.StopRecording
        });
        isRecording = false;
        timestamp = 0f;
        SaveRecordingDataInFile(signal.fileName);
    }

    private void RecordInput(RecordInputSignal signal)
    {
        if (isRecording)
        {
            recordingData.records.Add(new Record { timestamp = timestamp, buttonAnchoredPosition = signal.anchoredPosition, buttonName = signal.buttonName, inputType = signal.inputType, colorName = signal.colorName });
        }
    }

    #endregion

    #region Timer
    private void Update()
    {
        if(isRecording)
        {
            timestamp += Time.deltaTime;
        }
    }

    #endregion

    #region File Manipulation
    private void SaveRecordingDataInFile(string fileName)
    {
        string jsonString = JsonUtility.ToJson(recordingData);
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        File.WriteAllText(path, jsonString);
    }
    #endregion

}
