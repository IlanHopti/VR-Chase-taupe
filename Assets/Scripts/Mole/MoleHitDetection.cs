using UnityEngine;

public class MoleHitDetection : MonoBehaviour
{
    private bool isHit = false;
    public System.Action OnHitCallback; // Callback pour notifier le hit

    void OnTriggerEnter(Collider other)
    {
        if (!isHit && other.CompareTag("Hammer"))
        {
            isHit = true;
            HandleHit();
        }
    }

    void HandleHit()
    {
        GameManager.Instance.OnMoleHit(10); 
        OnHitCallback?.Invoke(); 
    }
}
