using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUpPrefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SpawnRandom(Vector3 position)
    {
        if (powerUpPrefabs.Length == 0) return;

        int index = UnityEngine.Random.Range(0, powerUpPrefabs.Length);
        var instance = Instantiate(powerUpPrefabs[index], position, Quaternion.identity);
        var powerUp = instance.GetComponent<IPowerUp>();
        GameEvents.RaisePowerUpSpawned(powerUp);
    }
}
