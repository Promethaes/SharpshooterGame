using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Crosshair : MonoBehaviour
{
    [Tooltip("References")]
    [SerializeField] Camera camera = null;
    Vector2 _pointerPos = Vector2.zero;
    public static bool draggingCrosshair = false;

    Vector2 _lastShotPosition = Vector2.zero;

    private void Start()
    {
        PlayerController.OnFire.AddListener(OnFire);
        BulletTimeManager.OnBulletReset.AddListener(OnBulletReset);
    }

    public void OnPointerMove(CallbackContext ctx)
    {
        _pointerPos = ctx.ReadValue<Vector2>();
        _pointerPos = camera.ScreenToWorldPoint(_pointerPos);
        if (draggingCrosshair)
            transform.position = new Vector3(_pointerPos.x, _pointerPos.y, 0.0f);
    }
    public void OnClick(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            var ray = Physics2D.Raycast(_pointerPos, new Vector2(0.0f, 0.0f));
            draggingCrosshair = ray.collider != null && ray.collider.CompareTag("Crosshair");
        }
        else
            draggingCrosshair = false;
    }

    public void OnFire()
    {
        _lastShotPosition = transform.localPosition;
        gameObject.SetActive(false);
    }

    public void OnBulletReset()
    {
        gameObject.SetActive(true);
        transform.localPosition = _lastShotPosition;
    }
}
