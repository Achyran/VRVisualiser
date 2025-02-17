using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyrcelVisualiser : MonoBehaviour
{
    [SerializeField]
    private int amount = 512;
    public GameObject prefab;
    private GameObject[] samplecubes;
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
    [SerializeField]
    private float cubeScale =1 ;
    void Start()
    {
        samplecubes = new GameObject[amount];
        if (Analyser.current == null)
        {
            Debug.LogWarning("An Analyser Is Needed");
            Destroy(this);
        }
        for (int i = 0; i < amount; i++)
        {
            GameObject g = (GameObject)Instantiate(prefab,this.transform);
            g.name = $"Visualise cube {i}";
            this.transform.eulerAngles = new Vector3(0, ((float) 360f/amount)* i, 0);
            g.transform.position = Vector3.forward * radius;
            g.transform.eulerAngles = new Vector3(0, 0, 0);
            samplecubes[i] = g;
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < amount; i++)
        {
            if (samplecubes != null)
            {
                switch (direction)
                {
                    case Direction.X:
                        samplecubes[i].transform.localScale = new Vector3(Analyser.current.samplesLeft[i] * scale, cubeScale, cubeScale);
                        break;
                    case Direction.Y:
                        samplecubes[i].transform.localScale = new Vector3(cubeScale, Analyser.current.samplesLeft[i] * scale, cubeScale);
                        break;
                    case Direction.Z:
                        samplecubes[i].transform.localScale = new Vector3(cubeScale,cubeScale, Analyser.current.samplesLeft[i] * scale);
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
