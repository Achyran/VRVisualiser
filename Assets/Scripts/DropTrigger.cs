using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DropTrigger : MonoBehaviour
{
    private ParticleSystem ps;
    private bool isLoaded;
    [SerializeField]
    [Range(0, 0.2f)]
    private float sensetivity;
    [SerializeField]
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Analyser.current.bandDifference[index] > sensetivity) ps.Play();
    }


}
