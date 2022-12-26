using UnityEngine;
using Zenject;

public class RecordAndReplayInstaller : MonoInstaller
{
    public RecordingSystem recordingSystem;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<RecordingSystem>().FromInstance(recordingSystem).AsSingle();
        RecordingSignals();
    }

    private void RecordingSignals()
    {
        Container.DeclareSignal<RecordDragging>();
    }
}