using UnityEngine;

public class UIManager : MonoBehaviour {
    public GameObject InputFeld;
    public GameObject YesNoOptions;
    public void ShowInputFeld() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        MainCharacter.Instance.canInteract = false;
        InputFeld.SetActive(true);
    }
    public void HideInputFeld() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MainCharacter.Instance.canInteract = true;
        InputFeld.SetActive(false);
    }
    public void ShowYesNoOptions() {
        Cursor.lockState = CursorLockMode.None;
        MainCharacter.Instance.canInteract = false;
        Cursor.visible = true;
        YesNoOptions.SetActive(true);
    }
    public void HideYesNoOptions() {
        Cursor.lockState = CursorLockMode.Locked;
        MainCharacter.Instance.canInteract = true;
        Cursor.visible = false;
        YesNoOptions.SetActive(false);
    }
}
