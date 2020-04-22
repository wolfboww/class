using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraBlack : MonoBehaviour
{
    public float targetBrightness;
    public float time;

    private BrightnessSaturationAndContrast component;

    private void OnEnable()
    {
        component = GetComponent<BrightnessSaturationAndContrast>();
        StartCoroutine(Brightness());
    }

    IEnumerator Brightness()
    {
        while (Mathf.Abs(component.brightness - targetBrightness) > 0.01f)
        {
            if (component.brightness < targetBrightness)
                yield return component.brightness += 0.1f;
            else
                yield return component.brightness -= 0.1f;

            yield return new WaitForSeconds(0.1f);
        }
        yield return component.enabled = false;
    }
}
