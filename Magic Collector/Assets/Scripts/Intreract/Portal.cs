using UnityEngine;

public class Portal : MonoBehaviour, Interactable
{
    [SerializeField] GameObject Player;
    [SerializeField] Transform teleportationPoint;
    CapsuleCollider capsuleCollider;
    CharacterController controller;
    void Start()
    {
        capsuleCollider = Player.GetComponent<CapsuleCollider>();
        controller = Player.GetComponent<CharacterController>();
    }
    public void Interact()
    {
        capsuleCollider.enabled = false;
        controller.enabled = false;
        Player.transform.position = teleportationPoint.position;
        controller.enabled = true;
        capsuleCollider.enabled = true;
    }
}
