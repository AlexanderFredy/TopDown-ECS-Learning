using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform trans = GetComponent<Transform>();

        //trans.DOScale(0.8f,2.5f).SetLoops(-1,LoopType.Yoyo).SetLink(gameObject).Play();
        trans.DORotate(new Vector3(0f,360, 0f),4f,RotateMode.FastBeyond360).SetRelative(true).SetLoops(-1).SetEase(Ease.Linear).SetLink(gameObject).Play();
    }
}
