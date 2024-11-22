using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class MoleTracker : MonoBehaviour
{
    public List<GameObject> molePrefabs; 
    public List<Transform> moleSpawnPoints; 

    private List<GameObject> activeMoles = new List<GameObject>();
    private List<GameObject> availableMoles;
    public static MoleTracker Instance;
    public int maxActiveMoles = 1; 
    public float baseSpeed = 1f; 
    public float multiplicator = 1f; 

    void Start()
    {
        availableMoles = new List<GameObject>(molePrefabs);
        StartSpawningMoles();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartSpawningMoles()
    {
        
        int activeMolesCount = Mathf.Min(maxActiveMoles, moleSpawnPoints.Count);

        for (int i = 0; i < activeMolesCount; i++)
        {
            Transform randomSpawnPoint = GetRandomSpawnPoint();
            SpawnMoleAtPoint(randomSpawnPoint);
        }
    }

    Transform GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, moleSpawnPoints.Count);
        return moleSpawnPoints[randomIndex];
    }

    void SpawnMoleAtPoint(Transform spawnPoint)
    {
       
        if (GameManager.Instance.isGameOver) return;

        if (availableMoles.Count == 0)
        {
            availableMoles = new List<GameObject>(molePrefabs);
        }

        int randomIndex = Random.Range(0, availableMoles.Count);
        GameObject selectedMole = availableMoles[randomIndex];
        availableMoles.RemoveAt(randomIndex);

        GameObject newMole = Instantiate(selectedMole, spawnPoint.position, Quaternion.Euler(-90, 0, 180));
        activeMoles.Add(newMole);

        float upHeight = spawnPoint.position.y + 0.334f;
        float upDuration = Random.Range(1f / baseSpeed, 1.5f / (baseSpeed * multiplicator));
        float downDuration = Random.Range(1f / baseSpeed, 1.5f / (baseSpeed * multiplicator));
        float stayDuration = Random.Range(1.5f / baseSpeed, (3f / multiplicator));
        float intervalBeforeNext = Random.Range(1f / baseSpeed, 3f / (baseSpeed * multiplicator));

        bool wasHit = false;

        Sequence moleSequence = DOTween.Sequence();
        moleSequence.Append(newMole.transform.DOMoveY(upHeight, upDuration))
                    .AppendInterval(stayDuration)
                    .Append(newMole.transform.DOMoveY(spawnPoint.position.y, downDuration))
                    .AppendInterval(intervalBeforeNext)
                    .OnComplete(() =>
                    {
                        if (wasHit) return; // Taupe déjà frappée
                        if (!GameManager.Instance.isGameOver) // Si le jeu n'est pas terminé
                        {
                            GameManager.Instance.OnMoleMissed();
                            activeMoles.Remove(newMole);
                            Destroy(newMole);
                            SpawnMoleAtPoint(GetRandomSpawnPoint());
                        }
                    });

        newMole.GetComponent<MoleHitDetection>().OnHitCallback = () =>
        {
            if (!wasHit)
            {
                wasHit = true;
                moleSequence.Kill();
                activeMoles.Remove(newMole);
                Destroy(newMole);

                if (!GameManager.Instance.isGameOver) // Vérifie que le jeu n'est pas terminé avant de respawner
                {
                    SpawnMoleAtPoint(GetRandomSpawnPoint());
                }
            }
        };
    }


    public void StopAllMoles()
    {
        foreach (GameObject mole in activeMoles)
        {
            if (mole != null)
            {
                Destroy(mole); 
            }
        }
        activeMoles.Clear(); 
    }

    public void ResetTracker()
    {
        multiplicator = 1f;
        baseSpeed = 1f;
        maxActiveMoles = 1;
        DOTween.KillAll();
        StopAllMoles(); 
    }



    public void IncreaseMaxActiveMoles()
    {
        if (maxActiveMoles < moleSpawnPoints.Count)
        {
            maxActiveMoles++;
        }
    }

    public void UpdateSpeedMultiplier(float multiplier)
    {
        multiplicator = multiplier;
    }
}
