using UnityEngine;
using Zenject;

public class RecordAndReplayInstaller : MonoInstaller
{
    public RecordingSystem recordingSystem;
    public ReplaySystem replaySystem;
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.BindInterfacesAndSelfTo<RecordingSystem>().FromInstance(recordingSystem).AsSingle();
        Container.BindInterfacesAndSelfTo<ReplaySystem>().FromInstance(replaySystem).AsSingle();
        RecordingSignals();
    }

    private void RecordingSignals()
    {
        Container.DeclareSignal<RecordInputSignal>();
        Container.DeclareSignal<StartRecording>();
        Container.DeclareSignal<StopRecording>();
        Container.DeclareSignal<PlayRecording>();
        Container.DeclareSignal<StartReplay>();
        Container.DeclareSignal<RecordInitialStateOfButtons>();
    }
}