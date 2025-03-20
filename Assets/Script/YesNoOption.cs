using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class YesNoOption : MonoBehaviour {
    [SerializeField] Button yes;
    [SerializeField] Button no;
    [SerializeField] UnityEvent yesEvent;
    [SerializeField] UnityEvent noEvent;

    private void Awake() {
        yes.onClick.AddListener(YesPressed);
        no.onClick.AddListener(NoPressed);
    }
    private void YesPressed() {
        yesEvent.Invoke();
    }
    private void NoPressed() {
        noEvent.Invoke();
    }
}
