using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairVisuals : MonoBehaviour
{
    [SerializeField] AnimationCurve lerpCurve = null;
    [SerializeField] Vector3 lerpToScale = new Vector3(0.5f, 0.5f, 0.5f);
    Vector3 _originalScale = Vector3.one;
    // Start is called before the first frame update
    void Start()
    {
        _originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        IEnumerator Lerp()
        {
            float x = 0.0f;

            while (true)
            {
                while (x < 1.0f)
                {
                    yield return new WaitForEndOfFrame();
                    x += Time.deltaTime;
                    transform.localScale = Vector3.Lerp(_originalScale, lerpToScale, lerpCurve.Evaluate(x));
                }
                x = 0.0f;
            }
        }
        StartCoroutine(Lerp());
    }
}
