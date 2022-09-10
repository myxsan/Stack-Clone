using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }
    public MoveDirection MoveDirection { get; set; }

    private void OnEnable()
    {
        CurrentCube = this;
        if(LastCube == null)
            LastCube = GameObject.Find("BaseCube").GetComponent<MovingCube>();

        GetComponent<Renderer>().material.color = GetRandomColor();
        transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);
    }
    private void OnDisable()
    {
        LastCube = this;
    }

    private Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
    }

    internal void Stop()
    {
        moveSpeed = 0;

        float hangOver = GetHangOver();

        float max = MoveDirection == MoveDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;
        if (Mathf.Abs(hangOver) >= max)
        {
            LastCube = null;
            CurrentCube = null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        int dir = hangOver > 0 ? 1 : -1;
        if (MoveDirection == MoveDirection.Z)
            SplitCubeOnZ(hangOver, dir);
        else
            SplitCubeOnX(hangOver, dir);
        this.enabled = false;
    }

    private float GetHangOver()
    {
        if(MoveDirection == MoveDirection.Z)
           return transform.position.z - Mathf.Abs(LastCube.transform.position.z);
        else
            return transform.position.x - Mathf.Abs(LastCube.transform.position.x);
    }

    private void SplitCubeOnX(float _hangOver, int _dir)
    {
        float newXSize = LastCube.transform.localScale.x - Mathf.Abs(_hangOver);
        float fallingBlockXSize = transform.localScale.x - newXSize;

        float newXPosition = LastCube.transform.localPosition.x + (_hangOver / 2f);

        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newXSize / 2f * _dir);
        float fallingBlockZposition = cubeEdge + (fallingBlockXSize / 2f * _dir);

        SpawnFallingCube(fallingBlockZposition, fallingBlockXSize);
    }

    private void SplitCubeOnZ(float _hangOver, int _dir)
    {
        float newZSize = LastCube.transform.localScale.z - Mathf.Abs(_hangOver);
        float fallingBlockZSize = transform.localScale.z - newZSize;

        float newZPosition = LastCube.transform.localPosition.z + (_hangOver / 2f);

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2f * _dir);
        float fallingBlockZposition = cubeEdge + (fallingBlockZSize / 2f * _dir);

        SpawnFallingCube(fallingBlockZposition, fallingBlockZSize);
    }

    private void SpawnFallingCube(float _fallingBlockZPosition, float _fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if(MoveDirection == MoveDirection.Z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, _fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, _fallingBlockZPosition);
        }
        else
        {
            cube.transform.localScale = new Vector3(_fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(_fallingBlockZPosition, transform.position.y, transform.position.z);
        }

        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(cube, 3f);
    }

    void FixedUpdate()
    {
        if(MoveDirection == MoveDirection.Z)
            transform.position += moveSpeed * Time.fixedDeltaTime * transform.forward;
        else
            transform.position += moveSpeed * Time.fixedDeltaTime * transform.right;
    }
}
