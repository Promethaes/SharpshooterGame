using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpScale : MonoBehaviour
{
    [SerializeField] AnimationCurve lerpCurve;
    [Tooltip("The scale to lerp to, in local space.")]
    [SerializeField] Vector2 lerpToScale;

    Vector2 _originalScale = Vector2.one;
    // Start is called before the first frame update
    void Start()
    {
        _originalScale = transform.localScale;
    }

    public void Lerp()
    {
        IEnumerator ILerp()
        {
            float x = 0.0f;
            while (x < 1.0f)
            {
                yield return new WaitForEndOfFrame();
                x += Time.deltaTime;

                transform.localScale = Vector3.Lerp(
                    new Vector3(_originalScale.x, _originalScale.y, 1.0f),
                    new Vector3(lerpToScale.x, lerpToScale.y, 1.0f),
                    lerpCurve.Evaluate(x));

            }
        }
        StartCoroutine(ILerp());

    }
}
