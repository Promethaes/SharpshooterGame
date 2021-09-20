using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpScale : MonoBehaviour
{
    [SerializeField] AnimationCurve lerpCurve = null;
    [SerializeField] float lerpScale = 0.25f;
    [SerializeField] float lerpSpeed = 4.0f;
    Vector2 _lerpToScale = Vector2.one;

    Vector2 _originalScale = Vector2.one;
    // Start is called before the first frame update
    void Start()
    {
        _originalScale = transform.localScale;
        _lerpToScale = _originalScale + new Vector2(lerpScale, lerpScale);
    }

    public void Lerp()
    {
        IEnumerator ILerp()
        {
            float x = 0.0f;
            while (x < 1.0f)
            {
                yield return new WaitForEndOfFrame();
                x += Time.deltaTime * lerpSpeed;

                transform.localScale = Vector3.Lerp(
                    new Vector3(_originalScale.x, _originalScale.y, 1.0f),
                    new Vector3(_lerpToScale.x, _lerpToScale.y, 1.0f),
                    lerpCurve.Evaluate(x));

            }
        }
        StartCoroutine(ILerp());

    }
}
