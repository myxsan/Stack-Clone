using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentSpawner : MonoBehaviour
{
    public MovingCube cubePrefab;
    public MoveDirection moveDirection;

    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab);

        if(MovingCube.CurrentCube != null && MovingCube.LastCube.gameObject != GameObject.Find("BaseCube"))
        {
            float x = moveDirection == MoveDirection.X ? transform.position.x : MovingCube.LastCube.transform.position.x;
            float z = moveDirection == MoveDirection.Z ? transform.position.z : MovingCube.LastCube.transform.position.z;


            cube.transform.position = new Vector3(x, 
                                                  MovingCube.LastCube.transform.position.y + cubePrefab.transform.localScale.y,
                                                  z);
        }
        else
        {
            cube.transform.position = transform.position;
        }

        cube.MoveDirection = moveDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
    }
}
