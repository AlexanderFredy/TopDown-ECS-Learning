using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InvisibleAbility : MonoBehaviour, IAbility
{
    public Material mainMaterial;
    public Material invisMaterial;
    public float duration;
    public float collDownDuration;
    public Renderer[] AllParts;
    public bool invisNow;
    private bool invisIsReady = true;
    private Image img;

    private void Awake()
    {
        img = GameObject.Find("Invisible").GetComponent<Image>();
        img.fillAmount = 1;
    }

    public void Execute()
    {
        if (!invisIsReady) return;

        if (invisMaterial != null)
        {
            invisNow = true;
            invisIsReady = false;
            StartCoroutine(ChangeCallDown());
            StartCoroutine(DoVisible());

            for (int k = 0; k < AllParts.Length; k++)
            {
                AllParts[k].material = invisMaterial;
            }

            FindObjectOfType<AttackBehaviour>().ClearTarget();
        }
        else
            Debug.Log("No invis mat prefab");
    }

    private IEnumerator ChangeCallDown()
    {
        img.fillAmount = 0;

        int steps = (int)(collDownDuration / 0.1f);
        float delta = (float)1 / steps;

        for (int i = 0; i < steps; i++)
        {
            img.fillAmount += delta;
            yield return new WaitForSeconds(0.1f);
        }

        invisIsReady = true;
    }

    private IEnumerator DoVisible()
    {
        yield return new WaitForSeconds(duration);

        for (int k = 0; k < AllParts.Length; k++)
        {
            AllParts[k].material = mainMaterial;
        }

        invisNow = false;
    }
}
