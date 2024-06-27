using System;
using UnityEngine;
using System.Threading.Tasks;

public class GrowEffect : IConsumeEffect, IDisposable
{
    private GameObject _subject;
    private float _growthScale;
    private float _duration;

    public GrowEffect(GameObject subject, float growthScale, float duration)
    {
        _subject = subject;
        _growthScale = growthScale;
        _duration = duration;

        Execute();
    }

    private async void Execute()
    {
        await StartEffect();
        Dispose();
    }

    private async Task StartEffect()
    {
        _subject.transform.localScale *= _growthScale;
        await Task.Delay((int)_duration*1000);
        _subject.transform.localScale /= _growthScale;
    }

    public void Dispose()
    {
        _subject.GetComponent<CharacterData>().currentConsumeEffects.Remove(this);
    }
}