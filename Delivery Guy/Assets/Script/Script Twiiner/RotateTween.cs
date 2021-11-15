using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTween : MonoBehaviour
{
    public iTween.EaseType easeType;
    public iTween.LoopType loopType;

    private void Start()
    {
        iTween.RotateTo(this.gameObject, iTween.Hash("z", 180, "time", 1f, "easetype", easeType, "looptype", loopType));
    }
}
