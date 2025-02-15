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
    private float yScale;

    enum Direction
    {
        X,Y,Z
    }
    [SerializeField]
    Direction direction;
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
                samplecubes[i].transform.localScale = new Vector3(samplecubes[i].transform.localScale.x, Analyser.current.samplesLeft[i] * yScale + 2, samplecubes[i].transform.localScale.z);
            }
        }
    }
}
