using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

public class ReplaySystem : MonoBehaviour
{
    #region Zenject
    private SignalBus _signalBus;

    [Inject]
    private void InjectSignalBus(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<StartReplay>(StartReplay);
    }
    private void StartReplay(StartReplay signal)
    {
        CheckIfFileExists(signal.fileName);
    }

    #endregion

    #region Fetch data from file
    private void CheckIfFileExists(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        if(File.Exists(path))
        {
            FetchDataAndStartReplay(path);
        }
        else
        {
            _signalBus.Fire(new FileDoesNotExistForReplay());
        }
    }

    private void FetchDataAndStartReplay(string path)
    {
        string jsonString = File.ReadAllText(path);
        Debug.Log($"read from file {path} {jsonString}");
        RecordingData rd = JsonUtility.FromJson<RecordingData>(jsonString);
        StartShowingReplay(rd);
    }
    #endregion

    #region Replaying
    private async void StartShowingReplay(RecordingData rd)
    {
        for(int i = 0; i < rd.records.Count - 1; ++i)
        {
            _signalBus.Fire(new PlayRecording
            {
                record = rd.records[i]
            });
            await UniTask.Delay((int)((rd.records[i + 1].timestamp - rd.records[i].timestamp) * 1000));
        }
        StopReplay();
    }

    private void StopReplay()
    {
        _signalBus.Fire(new SetBlockerState
        {
            enable = false
        });
        _signalBus.Fire(new ReplayCompleted());
    }
    #endregion
}
