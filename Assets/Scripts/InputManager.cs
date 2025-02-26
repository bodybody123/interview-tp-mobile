using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;
    [SerializeField]
    private LayerMask placementLayerMask;
    private Vector3 lastPosition;

    private PlayerInput playerInput;

    public event Action OnClicked, OnExit;

    void Update()
    {
      if (Input.GetMouseButtonDown(0)) {
        OnClicked?.Invoke();
      }  

      if (Input.GetMouseButtonDown(2)) {
        OnExit?.Invoke();
      }
    }

    void Awake() {
        playerInput = GetComponent<PlayerInput>();
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition() {
        Vector3 pos = playerInput.actions["Position"].ReadValue<Vector2>();
        Vector3 mousePos = Input.mousePosition;
        Ray ray = sceneCamera.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, placementLayerMask)) {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}
