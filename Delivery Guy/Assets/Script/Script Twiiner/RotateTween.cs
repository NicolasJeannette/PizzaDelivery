using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class RotateTween : MonoBehaviour
{
    public float variableRotate;
    public float duration;

    private void Start()
    {
        this.transform.DORotate(Vector3.up * variableRotate, duration,RotateMode.WorldAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
    }
}
