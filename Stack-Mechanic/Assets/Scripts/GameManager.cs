using UnityEngine;

public class GameManager : MonoBehaviour
{
    int fireButtonID;
    private SegmentSpawner[] spawners;
    private int spawnerIndex;
    private SegmentSpawner currentSpawner;
    private void Awake()
    {
        fireButtonID = Input.GetButton("Fire1").GetHashCode();
        spawners = FindObjectsOfType<SegmentSpawner>();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(fireButtonID))
        {
            if(MovingCube.CurrentCube != null)
                MovingCube.CurrentCube.Stop();
            
            spawnerIndex = spawnerIndex == 0 ? 1 : 0;
            currentSpawner = spawners[spawnerIndex];

            currentSpawner.SpawnCube();
        }
    }
}
