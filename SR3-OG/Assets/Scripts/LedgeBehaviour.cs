using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LedgeBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<CharacterController>(out CharacterController controller))
        {
            controller.SendMessage("LedgeHit", transform);
        }
    }
}