using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollControl : MonoBehaviour
{
    public Animator Animator;
    public Rigidbody[] AllRigidbodies;

    void Awake()
    {
        for (int i = 0; i < AllRigidbodies.Length; i++)
        {
            AllRigidbodies[i].isKinematic = true;
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.Space))
    //    {
    //        RagdollEnable();
    //    }
    //}

    public void RagdollEnable()
    {
        Animator.enabled = false;

        for (int i = 0; i < AllRigidbodies.Length; i++)
        {
            AllRigidbodies[i].isKinematic = false;
        }
    }
}
