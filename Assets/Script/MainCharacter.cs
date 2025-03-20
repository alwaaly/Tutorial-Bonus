using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainCharacter : MonoBehaviour {
    [SerializeField] CharacterController controller;
    [SerializeField] TalkSequencer sequencer;
    [SerializeField] float movmenSpeed = 6;
    [SerializeField] Transform cam;
    InputSystem_Actions action;
    Vector2 inputMovmentVector;
    bool canMove;
    [SerializeField] LayerMask mask;
    [SerializeField] Image dot;
    public bool canInteract;
    public static MainCharacter Instance;
    private void Awake() {
        Instance= this;
        canInteract = true;
        action = new();
        action.Enable();
        action.Player.Move.performed += Move_performed;
        action.Player.Move.canceled += Move_canceled;
        action.Player.Interact.started += Interact_started;
        canMove = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Interact_started(InputAction.CallbackContext obj) {
        if (!canInteract) return;
        Ray camRay = Camera.main.ScreenPointToRay(new(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(camRay, out RaycastHit hitInfo, 5, mask)) {
            if (hitInfo.transform.TryGetComponent(out IInteractable interactable))
                interactable.OnInteract();
        }
    }

    private void FixedUpdate() {
        Ray camRay = Camera.main.ScreenPointToRay(new(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(camRay,5, mask)) {
            dot.color = Color.red;
        }else if(dot.color == Color.red) {
            dot.color = Color.white;
        }
    }

    private void Move_canceled(InputAction.CallbackContext context) {
        inputMovmentVector = context.ReadValue<Vector2>();
    }

    private void Move_performed(InputAction.CallbackContext context) {
        inputMovmentVector = context.ReadValue<Vector2>();
    }

    public void Freeze() {
        canMove = false;
    }
    public void UnFreeze() {
        canMove = true;
    }

    void Update() {
        if (!canMove) return;
        Vector3 right = cam.right * inputMovmentVector.x;
        right.y = 0;
        Vector3 forward = cam.forward * inputMovmentVector.y;
        forward.y = 0;
        Vector3 direction = right + forward;
        direction.Normalize();
        controller.Move((direction * movmenSpeed + (Vector3.up * -3f)) * Time.deltaTime);
    }
    public void OnSetPlayerName(string name) {
        sequencer.signatures.Add(new('$', name));
    }
    private void OnDisable() {
        action.Disable();
    }
}
