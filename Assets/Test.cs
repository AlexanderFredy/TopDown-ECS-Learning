using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        print("awake");
    }


    // Start is called before the first frame update
    void Start()
    {
        print("start");
    }

    // Update is called once per frame
    void Update()
    {
        print("update");
    }

    private void OnEnable()
    {
        print("enable");
    }

    private void OnDisable()
    {
        print("disable");
    }
}
