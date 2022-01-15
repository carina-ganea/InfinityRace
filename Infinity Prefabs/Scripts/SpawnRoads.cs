using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoads : MonoBehaviour
{
    
    public GameObject[] Roads;
    public GameObject Map;
    public Transform Player;
    private GameObject currentRoad;
    Vector3[] lastSpawn = { new Vector3(0, 0, 0) };
    private int indexCurrent;

    // Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(0, Roads.Length);
        indexCurrent = index;
        currentRoad = (GameObject) Instantiate(Roads[index], new Vector3(0, 0, 0), Quaternion.identity, Map.transform);

        


        Instantiate(Roads[index], new Vector3(0, 0,  250.0F), Quaternion.identity, Map.transform);

        Instantiate(Roads[index], new Vector3(250.0F, 0, 0), Quaternion.identity, Map.transform);

        Instantiate(Roads[index], new Vector3(0, 0, - 250.0F), Quaternion.identity, Map.transform);

        Instantiate(Roads[index], new Vector3( - 250.0F, 0, 0), Quaternion.identity, Map.transform);


        
    }

    // Update is called once per frame
    void Update()
    {
        float X, Z;
        X = Player.position.x;
        Z = Player.position.z;

        for( int i = 0; i < Map.transform.childCount; i++)
        {
            GameObject newCurrentRoad = Map.transform.GetChild(i).gameObject;
            if (newCurrentRoad != currentRoad && newCurrentRoad.transform.position.x + 250 > X && newCurrentRoad.transform.position.x < X && newCurrentRoad.transform.position.z + 250 > Z && newCurrentRoad.transform.position.z < Z)
            {
                int index = 0;
                for (int j = 0; j < Map.transform.childCount; j++)
                {
                    if (!Map.transform.GetChild(j).gameObject.Equals(newCurrentRoad) && !currentRoad.Equals(Map.transform.GetChild(j).gameObject))
                    {
                        Object.Destroy(Map.transform.GetChild(j).gameObject);
                    }
                }

                
                if (currentRoad.transform.position.x != newCurrentRoad.transform.position.x || currentRoad.transform.position.z != newCurrentRoad.transform.position.z + 250.0F)
                    Instantiate(Roads[index], new Vector3(newCurrentRoad.transform.position.x, 0, newCurrentRoad.transform.position.z + 250.0F), Quaternion.identity, Map.transform);

                if (currentRoad.transform.position.x != newCurrentRoad.transform.position.x + 250.0F || currentRoad.transform.position.z != newCurrentRoad.transform.position.z)
                    Instantiate(Roads[index], new Vector3(newCurrentRoad.transform.position.x + 250.0F, 0, newCurrentRoad.transform.position.z), Quaternion.identity, Map.transform);

                if (currentRoad.transform.position.x != newCurrentRoad.transform.position.x || currentRoad.transform.position.z != newCurrentRoad.transform.position.z - 250.0F)
                    Instantiate(Roads[index], new Vector3(newCurrentRoad.transform.position.x, 0, newCurrentRoad.transform.position.z - 250.0F), Quaternion.identity, Map.transform);

                if (currentRoad.transform.position.x != newCurrentRoad.transform.position.x - 250.0F || currentRoad.transform.position.z != newCurrentRoad.transform.position.z)
                    Instantiate(Roads[index], new Vector3(newCurrentRoad.transform.position.x - 250.0F, 0, newCurrentRoad.transform.position.z), Quaternion.identity, Map.transform);

                currentRoad = newCurrentRoad;
                indexCurrent = index;
                break;
            }
        }
    }
}
