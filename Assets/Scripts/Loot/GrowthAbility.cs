
public class GrowthAbility : BaseItem, IConsumable, ICraftable
{
    public float growthScale = 2f;
    public float duration = 5f;

    public IngredientType IngredientType => IngredientType.red;

    public void Consume()
    {
        if (Owner != null)
        {
            var effect = new GrowEffect(Owner, growthScale, duration);
            Owner.GetComponent<CharacterData>().currentConsumeEffects.Add(effect); 
        }
    }
}
