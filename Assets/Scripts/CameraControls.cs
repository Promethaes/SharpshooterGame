using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CameraControls : MonoBehaviour
{
    [SerializeField] AnimationCurve cameraMovementAndZoomCurve = null;
    [SerializeField] float scrollSpeed = 1.0f;
    [SerializeField] float cameraZoomLowerBound = 5.0f;
    [SerializeField] float cameraZoomUpperBound = 10.0f;
    [SerializeField] float zoomSpeed = 1.0f;
    [SerializeField] float zoomTime = 0.25f;
    [Tooltip("References")]
    [SerializeField] Camera camera = null;

    Vector2 _startingCameraPos = Vector2.zero;

    bool _canDrag = true;
    Vector2 _pointerPos = Vector2.zero;
    bool _clicking = false;
    Vector2 _originalPointerPos = Vector2.zero;
    Vector2 _originalCameraPos = Vector2.zero;

    float _cameraZoomFactor = 0.0f;
    int _zoomSign = 0;
    Coroutine _zoomTimeCoroutine = null;

    private void Start()
    {
        _startingCameraPos = camera.transform.localPosition;
        PlayerController.OnFire.AddListener(OnFire);
        BulletTimeManager.OnBulletReset.AddListener(OnBulletReset);
        _cameraZoomFactor = 1.0f;
    }

    private void Update()
    {
        //zoom
        _cameraZoomFactor += _zoomSign * zoomSpeed * Time.deltaTime;
        _cameraZoomFactor = Mathf.Clamp(_cameraZoomFactor, 0.0f, 1.0f);
        camera.orthographicSize = Mathf.Lerp(cameraZoomLowerBound, cameraZoomUpperBound, cameraMovementAndZoomCurve.Evaluate(_cameraZoomFactor));
    }

    public void OnPointerMove(CallbackContext ctx)
    {
        _pointerPos = camera.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
        if (_clicking && _canDrag && !Crosshair.draggingCrosshair)
        {
            camera.transform.localPosition = _originalCameraPos - (_pointerPos - _originalPointerPos) * scrollSpeed;
            var pos = camera.transform.localPosition;
            pos.z = -2.0f;//ensure no weird z order bugs
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

    public void OnZoom(CallbackContext ctx)
    {
        var v = ctx.ReadValue<float>();
        if (v > 0.0f)
            _zoomSign = 1;
        else if (v < 0.0f)
            _zoomSign = -1;

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(zoomTime);
            _zoomSign = 0;
        }
        if (_zoomTimeCoroutine != null)
            StopCoroutine(_zoomTimeCoroutine);
        _zoomTimeCoroutine = StartCoroutine(Wait());
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
                var temp = Vector2.Lerp(originalPos, lerpTo, cameraMovementAndZoomCurve.Evaluate(x));
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
