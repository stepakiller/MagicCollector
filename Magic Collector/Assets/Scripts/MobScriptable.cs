using UnityEngine;

[CreateAssetMenu(fileName = "New mob", menuName = "ScriptableObjects/Mob")]
public class MobScriptable : ScriptableObject
{
    public Sprite sprite;
    public GameObject prefab;
    public string nameMob;
}
