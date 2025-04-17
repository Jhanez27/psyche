using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatModifierSO", menuName = "Scriptable Objects/CharacterStatModifierSO")]
public abstract class CharacterStatModifierSO : ScriptableObject
{
    public abstract void AffectCharacter(GameObject gameObject, float value);

}
