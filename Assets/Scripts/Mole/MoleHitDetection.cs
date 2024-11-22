using UnityEngine;

public class MoleHitDetection : MonoBehaviour
{
    private bool isHit = false;
    public System.Action OnHitCallback; 

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
        gameObject.GetComponent<AudioSource>().Play();
        OnHitCallback?.Invoke(); 
    }
}
