using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public int tileSize = 6;
    public int tileOffset = 1;
    GameObject[,] cubes;
    bool[,] stateMap;
    public float updateTime = 1000.0f;
    float period = 0.0f;
    void Start()
    {
        SetupMap();
        RandomizeMap();
    }

    void RandomizeMap()
    {
        for (int i = 0; i < 100; i++)
        {
              ToggleState(Random.Range(tileOffset, tileSize - tileOffset), Random.Range(tileOffset, tileSize - tileOffset));
        }
    }

    void SetupMap()
    {
        stateMap = new bool[tileSize, tileSize];
        cubes = new GameObject[tileSize, tileSize];
        GameObject cubeInstance = Instantiate(Resources.Load("Prefabs/Cube", typeof(GameObject))) as GameObject;

        for (int i = 0; i < tileSize; i++)
        {
            for (int j = 0; j < tileSize; j++)
            {
                GameObject cubeClone = Instantiate(cubeInstance, new Vector3(i, j, 0), cubeInstance.transform.rotation);
                cubeClone.name = "CubeClone-" + (i + 1) + "-" + (j + 1);
                cubes[i, j] = cubeClone;
                stateMap[i, j] = true;
            }
        }
    }

    void Update()
    {
        if (period > (updateTime/1000))
        {
            period = 0.0f;
            Process();
        }
        period += UnityEngine.Time.deltaTime;

    }

    void ToggleState(int i,int j)
    {
        stateMap[i, j] = !stateMap[i, j];
    }

    void ApplyStateMap()
    {

        for (int i = tileOffset; i < tileSize - tileOffset; i++)
        {
            for (int j = tileOffset; j < tileSize - tileOffset; j++)
            {
                stateMap[i, j] = cubes[i, j].activeSelf;
            }
        }
    }

    bool CanLive(int i, int j)
    {
        int neighbourCount = 0;
        for (int a = i-1; a < i+1; a++)
        {
            for(int b = j-1; b < j+1; b++)
            {
                if ((a == i) && (b == j))
                {
                    //Debug.Log(a+" -x- "+b);
                    continue;
                }
                if (stateMap[a, b])
                {
                    neighbourCount++;
                }
            }
        }

        if(i == 1 && j == 18)
        {
            Debug.Log("018 "+neighbourCount);
        }

        //Debug.Log(i+" "+ j+ " - "+neighbourCount);
        if(!stateMap[i, j])
        {
            if (neighbourCount == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (neighbourCount == 2 || neighbourCount == 3)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    void Process()
    {

        for(int i=tileOffset;i<tileSize-tileOffset;i++)
        {
            for(int j=tileOffset;j<tileSize-tileOffset;j++)
            {
                if (CanLive(i, j))
                {
                    cubes[i, j].SetActive(true);
                }else
                {
                    cubes[i, j].SetActive(false);
                }
            }
        }

        ApplyStateMap();
    }


}
