using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkSpirit : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float frequency = 20f;
    [SerializeField]
    private float magnitude = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(Mathf.Sin(Time.time * frequency) * magnitude,1,0) * speed * Time.deltaTime);
    }
}
