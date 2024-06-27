using System;
using UnityEngine;
using System.Threading.Tasks;

public class PoisonEffect : IConsumeEffect, IDisposable
{
    private GameObject _subject;
    public int _dps;
    public float _duration;

    public PoisonEffect(GameObject subject, int dps, float duration)
    {
        _subject = subject;
        _dps = dps;
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
        float startTime = Time.time;
        if (_subject.TryGetComponent(out CharacterHealth health))
        {
            while (Time.time - startTime < _duration)
            {
                health.ApplyDamage(_dps);
                await Task.Delay(1000);
            }
        }
    }

    public void Dispose()
    {
        _subject.GetComponent<CharacterData>().currentConsumeEffects.Remove(this);
    }
}