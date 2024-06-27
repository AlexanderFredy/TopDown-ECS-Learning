using DG.Tweening;
using UnityEngine;

public class SpikesAnimation : MonoBehaviour
{
    private Transform trans;
    private Sequence sequence;

    [SerializeField] private TrapAbility trap;
    // Start is called before the first frame update
    
    private void OnEnable()
    {
        trans = GetComponent<Transform>();
        trap.SetArmed(true);
        
        sequence = DOTween.Sequence();
        sequence.Append(trans.DOLocalMoveY(0.03f,0.3f));
        sequence.AppendCallback(() => trap.SetArmed(false));
        sequence.AppendInterval(1.5f);
        sequence.Append(trans.DOLocalMoveY(-0.3f,1.5f));
        sequence.AppendCallback(() => trap.SetArmed(true));
        
        trap.OnExecute += () => sequence.Restart();
    }

    private void OnDisable()
    {
        trap.OnExecute -= () => sequence.Restart();
    }
}
