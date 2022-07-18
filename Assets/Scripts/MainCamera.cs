using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private float _shakeLength = 0.3f;

    public void CameraShake()
    {
        StartCoroutine(CameraShakeRoutine());
    }

    private IEnumerator CameraShakeRoutine()
    {
        Vector3 _defaultCameraPos = this.transform.position;
        float _shakeTime = Time.time + _shakeLength;
        float _shakePosRangeMax = 0.1f; 

        while (Time.time < _shakeTime)
        {
            float _randomX = Random.Range(-_shakePosRangeMax, _shakePosRangeMax);
            float _randomY = Random.Range(-_shakePosRangeMax, _shakePosRangeMax);
            this.transform.position = new Vector3(_randomX, _randomY, transform.position.z);
            yield return new WaitForEndOfFrame();
        }

        this.transform.position = _defaultCameraPos;
    }
}
