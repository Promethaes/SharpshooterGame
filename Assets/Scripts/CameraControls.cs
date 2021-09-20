using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CameraControls : MonoBehaviour
{
    [SerializeField] AnimationCurve cameraScrollCurve;
    [SerializeField] float scrollSpeed = 1.0f;
    [Tooltip("References")]
    [SerializeField] Camera camera;

    Vector2 _startingCameraPos = Vector2.zero;

    bool _canDrag = true;
    Vector2 _pointerPos = Vector2.zero;
    bool _clicking = false;
    Vector2 _originalPointerPos = Vector2.zero;
    Vector2 _originalCameraPos = Vector2.zero;

    private void Start()
    {
        _startingCameraPos = camera.transform.localPosition;
        PlayerController.OnFire.AddListener(OnFire);
        BulletTimeManager.OnBulletReset.AddListener(OnBulletReset);
    }

    public void OnPointerMove(CallbackContext ctx)
    {
        _pointerPos = camera.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
        _pointerPos = camera.transform.TransformVector(_pointerPos);
        if (_clicking && _canDrag && !Crosshair.draggingCrosshair)
        {
            camera.transform.localPosition = _originalCameraPos - (_pointerPos - _originalPointerPos) * scrollSpeed;
            var pos = camera.transform.localPosition;
            pos.z = -2.0f;
            camera.transform.localPosition = pos;
        }
    }
    public void OnClick(CallbackContext ctx)
    {
        _clicking = ctx.performed;
        if (_clicking && _originalPointerPos == Vector2.zero)
        {
            _originalPointerPos = _pointerPos;
            _originalCameraPos = camera.transform.localPosition;
        }
        else if (ctx.canceled)
        {
            _originalPointerPos = Vector2.zero;
            _originalCameraPos = Vector2.zero;
        }

    }

    public void ResetCameraPoisition(bool lerpToOrigin)
    {
        IEnumerator Lerp(bool toOrigin)
        {
            float x = 0.0f;
            Vector2 originalPos = camera.transform.localPosition;
            var lerpTo = toOrigin ? Vector2.zero : _startingCameraPos;
            while (x < 1.0f)
            {
                yield return new WaitForEndOfFrame();
                x += Time.deltaTime;
                var temp = Vector2.Lerp(originalPos, lerpTo, cameraScrollCurve.Evaluate(x));
                Vector3 pos = temp;
                pos.z = -2.0f;
                camera.transform.localPosition = pos;
            }
        }
        StartCoroutine(Lerp(lerpToOrigin));
    }

    public void OnFire()
    {
        _canDrag = false;
        ResetCameraPoisition(true);
    }
    public void OnBulletReset()
    {
        _canDrag = true;
        ResetCameraPoisition(false);
    }
}
