using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float dropDistance;
    GameObject prefab;
    bool _IsBussy = false;
    public bool IsBussy => _IsBussy;
    public static InventoryController Instance { get; private set; }
    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(Settings.dropKey) && _IsBussy) DropItem(); 
    }
    public void ChangeSprite(Sprite sprite, GameObject obj)
    {
        image.sprite = sprite;
        prefab = obj;
        _IsBussy = true;
    }

    public void DropItem()
    {
        image.sprite = null;
        _IsBussy = false;
        Vector3 spawnPosition = transform.position + transform.forward * dropDistance;
        Instantiate(prefab, spawnPosition, Quaternion.identity);
        prefab = null;
    }
}
