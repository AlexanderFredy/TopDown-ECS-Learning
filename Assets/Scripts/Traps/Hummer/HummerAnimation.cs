using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HummerAnimation : MonoBehaviour
{
    private Transform trans;

    [SerializeField] private TrapAbility trap;
    [SerializeField] private AK.Wwise.Event fallEvent;

    void Start()
    {
        trans = GetComponent<Transform>();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(trans.DORotate(new Vector3(-60,0,0),2.5f,RotateMode.LocalAxisAdd));
        sequence.AppendCallback(() => trap.SetArmed(true));
        sequence.Append(trans.DORotate(new Vector3(60,0,0),0.4f,RotateMode.LocalAxisAdd));
        sequence.AppendCallback(() =>
        {
            trap.SetArmed(false);
            fallEvent.Post(gameObject);
        });
        sequence.AppendInterval(1f);
        sequence.SetLoops(-1);
        sequence.Play();
    }
    
}
