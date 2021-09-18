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
        _startingCameraPos = camera.transform.position;
    }

    public void OnPointerMove(CallbackContext ctx)
    {
        _pointerPos = camera.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
        if (_clicking && _canDrag)
            camera.transform.position = _originalCameraPos - (_pointerPos - _originalPointerPos) * scrollSpeed;
    }
    public void OnClick(CallbackContext ctx)
    {
        _clicking = ctx.performed;
        if (_clicking && _originalPointerPos == Vector2.zero)
        {
            _originalPointerPos = _pointerPos;
            _originalCameraPos = camera.transform.position;
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
            var originalPos = camera.transform.position;
            var lerpTo = toOrigin ? Vector2.zero : _startingCameraPos;
            while (x < 1.0f)
            {
                yield return new WaitForEndOfFrame();
                x += Time.deltaTime;
                camera.transform.position = Vector2.Lerp(originalPos, lerpTo, cameraScrollCurve.Evaluate(x));
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
    }
}
