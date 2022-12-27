using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UISystem : MonoBehaviour
{
    public Button recordButton, replayButton;
    public TextMeshProUGUI recordButtonText;
    public TMP_InputField fileNameInputField;
    public GameObject startRecordingIcon, stopRecordingIcon;

    private bool startRecording;

    private SignalBus _signalBus;

    [Inject]
    private void InjectSignalBus(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Start()
    {
        SetButtons();
        SetInputField();
    }

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
        _signalBus.Fire(new StartReplay
        {
            fileName = fileNameInputField.text
        });
    }
}
