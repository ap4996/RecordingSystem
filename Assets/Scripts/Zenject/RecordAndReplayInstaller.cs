using UnityEngine;
using Zenject;

public class RecordAndReplayInstaller : MonoInstaller
{
    public RecordingSystem recordingSystem;
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.BindInterfacesAndSelfTo<RecordingSystem>().FromInstance(recordingSystem).AsSingle();
        RecordingSignals();
    }

    private void RecordingSignals()
    {
        Container.DeclareSignal<RecordDragging>();
        Container.DeclareSignal<StartRecording>();
        Container.DeclareSignal<StopRecording>();
    }
}