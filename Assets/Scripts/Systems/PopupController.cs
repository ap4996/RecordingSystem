using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PopupController : MonoBehaviour
{
    public TextMeshProUGUI popupContentText;
    public Button closeButton;

    #region Zenject
    private SignalBus _signalBus;

    [Inject]
    private void InjectSignalBus(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<OpenPopup>(OpenPopup);
        _signalBus.Subscribe<PlayRecording>(PlayRecording);
    }
    private void OnDestroy()
    {
        _signalBus.TryUnsubscribe<OpenPopup>(OpenPopup);
        _signalBus.TryUnsubscribe<PlayRecording>(PlayRecording);
    }
    private void PlayRecording(PlayRecording signal)
    {
        if(signal.record.inputType == InputType.PopupClosed)
        {
            ClosePopup();
        }
    }

    private void OpenPopup(OpenPopup signal)
    {
        popupContentText.text = signal.popupContent;
        gameObject.SetActive(true);
    }

    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        SetButton();
    }
    #endregion

    #region Popup Manipulation
    private void SetButton()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(ClosePopup);
    }

    private void ClosePopup()
    {
        gameObject.SetActive(false);
        _signalBus.Fire(new RecordInputSignal
        {
            inputType = InputType.PopupClosed
        });
    }
    #endregion
}
