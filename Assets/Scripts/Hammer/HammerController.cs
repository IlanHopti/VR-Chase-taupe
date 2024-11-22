using UnityEngine;

public class HammerController : MonoBehaviour
{
    public Transform respawnPoint; 
    public float respawnDistance = 5f; 
    public float idleTimeBeforeRespawn = 15f; 

    private Rigidbody hammerRigidbody;
    private float timeSinceLastGrab; 
    private bool isHeld = false; 

    void Start()
    {
        hammerRigidbody = GetComponent<Rigidbody>();
        ResetIdleTimer();
    }

    void Update()
    {
        if (!isHeld)
        {
            timeSinceLastGrab += Time.deltaTime;

        
            if (Vector3.Distance(transform.position, respawnPoint.position) > respawnDistance || timeSinceLastGrab > idleTimeBeforeRespawn)
            {
                RespawnHammer();
            }
        }
    }

    public void OnGrab()
    {
        isHeld = true;
        hammerRigidbody.isKinematic = true; 
        ResetIdleTimer();
    }

    public void OnRelease()
    {
        isHeld = false;
        hammerRigidbody.isKinematic = false; 
        hammerRigidbody.useGravity = true; 
    }

    private void RespawnHammer()
    {
        
        hammerRigidbody.isKinematic = true; 
        hammerRigidbody.useGravity = false;
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation; 
        ResetIdleTimer();
    }

    private void ResetIdleTimer()
    {
        timeSinceLastGrab = 0f;
    }
}
