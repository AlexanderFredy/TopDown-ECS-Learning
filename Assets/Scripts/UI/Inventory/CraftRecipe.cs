using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CraftRecipe", menuName = "CraftRecipe")]
public class CraftRecipe: SerializedScriptableObject
{
    public BaseItem product;

    [OdinSerialize]
    public ICraftable[] ingredients = new ICraftable[2];
}