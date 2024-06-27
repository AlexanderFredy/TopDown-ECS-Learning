
public class PoisonAbility : BaseItem, IConsumable
{
    public int dps = 10;
    public float duration = 5f;

    public void Consume()
    {
        if (Owner != null)
        {
            var effect = new PoisonEffect(Owner, dps, duration);
            Owner.GetComponent<CharacterData>().currentConsumeEffects.Add(effect);
        }
    }
}
