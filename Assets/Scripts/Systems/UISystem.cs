using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UISystem : MonoBehaviour
{
    public Button recordButton, replayButton;
    public TextMeshProUGUI recordButtonText, replayButtonText;
    public TMP_InputField fileNameInputField;
    public GameObject startRecordingIcon, stopRecordingIcon, blockingOverlay;

    private bool startRecording;

    #region Zenject

    private SignalBus _signalBus;

    [Inject]
    private void InjectSignalBus(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<SetBlockerState>(SetBlockerState);
        _signalBus.Subscribe<FileDoesNotExistForReplay>(CannotReplay);
        _signalBus.Subscribe<ReplayCompleted>(ReplayCompleted);
    }

    private void OnDestroy()
    {
        _signalBus.TryUnsubscribe<SetBlockerState>(SetBlockerState);
        _signalBus.TryUnsubscribe<FileDoesNotExistForReplay>(CannotReplay);
        _signalBus.TryUnsubscribe<ReplayCompleted>(ReplayCompleted);
    }

    private void SetBlockerState(SetBlockerState signal)
    {
        blockingOverlay.SetActive(signal.enable);
    }

    private void CannotReplay()
    {
        _signalBus.Fire(new SetBlockerState
        {
            enable = false
        });
        replayButtonText.text = $"{fileNameInputField.text} File does not exists";
    }

    private void ReplayCompleted()
    {
        replayButtonText.text = "REPLAY";
    }
    #endregion

    #region Monobehaviour
    private void Start()
    {
        SetButtons();
        SetInputField();
    }
    #endregion

    #region UI Manipulation
    private void SetButtons()
    {
        recordButton.onClick.RemoveAllListeners();
        recordButton.onClick.AddListener(RecordButtonDelegate);

        replayButton.onClick.RemoveAllListeners();
        replayButton.onClick.AddListener(ReplayButtonDelegate);
    }

    private void SetInputField()
    {
        fileNameInputField.text = "RecordingFile";
    }

    private void RecordButtonDelegate()
    {
        SetRecordButtonState();
        if(startRecording) 
        {
            _signalBus.Fire(new StartRecording());
        }
        else
        {
            _signalBus.Fire(new StopRecording { fileName = fileNameInputField.text });
        }
    }

    private void SetRecordButtonState()
    {
        startRecording = !startRecording;
        startRecordingIcon.SetActive(!startRecording);
        stopRecordingIcon.SetActive(startRecording);
        recordButtonText.text = !startRecording ? "START RECORDING" : "STOP RECORDING";
    }

    private void ReplayButtonDelegate()
    {
        replayButtonText.text = "PLAYING REPLAY";
        _signalBus.Fire(new SetBlockerState
        {
            enable = true
        });
        _signalBus.Fire(new StartReplay
        {
            fileName = fileNameInputField.text
        });
    }
    #endregion
}
