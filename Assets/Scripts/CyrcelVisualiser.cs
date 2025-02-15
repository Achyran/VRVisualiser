using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyrcelVisualiser : MonoBehaviour
{
    
    public GameObject prefab;
    private GameObject[] samplecubes = new GameObject[512];
    [SerializeField]
    [Range(0,100)]
    private float radius;
    [SerializeField]
    private float scale;

    enum Direction
    {
        X,Y,Z
    }
    [SerializeField]
    Direction direction = Direction.X;
    void Start()
    {
        if(Analyser.current == null)
        {
            Debug.LogWarning("An Analyser Is Needed");
            Destroy(this);
        }
        for (int i = 0; i < 512; i++)
        {
            GameObject g = (GameObject)Instantiate(prefab,this.transform);
            g.name = $"Visualise cube {i}";
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            g.transform.position = Vector3.forward * radius;
            samplecubes[i] = g;
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 512; i++)
        {
            if (samplecubes != null)
            {
                switch (direction)
                {
                    case Direction.X:
                        samplecubes[i].transform.localScale = new Vector3(Analyser.current.samplesLeft[i] * scale, samplecubes[i].transform.y, samplecubes[i].transform.localScale.z);
                        break;
                    case Direction.Y:
                        samplecubes[i].transform.localScale = new Vector3(samplecubes[i].transform.localScale.x, Analyser.current.samplesLeft[i] * scale, samplecubes[i].transform.localScale.z);
                        break;
                    case Direction.Z:
                        samplecubes[i].transform.localScale = new Vector3(samplecubes[i].transform.localScale.x, samplecubes[i].transform.y, Analyser.current.samplesLeft[i] * scale);
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
