using UnityEngine;

public class Mob : MonoBehaviour, Interactable
{
    [SerializeField] MobScriptable mob;
    public void Interact()
    {
        if (!InventoryController.Instance.IsBussy)
        {
            InventoryController.Instance.ChangeSprite(mob.sprite, mob.prefab);
            Destroy(gameObject);
        }
    }
}
