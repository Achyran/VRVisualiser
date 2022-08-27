using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Audio;

public class LoopBackVisualize : MonoBehaviour
{
    [SerializeField]
    private AudioVisualizationStrategy strat;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private LoopbackAudio analyser;
    private List<GameObject> cubes;
    // Start is called before the first frame update
    void Start()
    {
        cubes = new List<GameObject>();
        for (int i = 0; i < analyser.SpectrumData.Length; i++)
        {
            cubes.Add(Instantiate(prefab, this.transform));
            cubes[i].transform.position += new Vector3(i, 0, 0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        float[] data = analyser.GetAllSpectrumData(strat);
        //float data = analyser.GetSpectrumData(strat);
        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].transform.localScale = new Vector3(1, data[i], 1); 
        }
    }
}
