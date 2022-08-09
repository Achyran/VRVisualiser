using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paramCube : MonoBehaviour
{
    [SerializeField]
    private int band;
    [SerializeField]
    private float startScale;
    [SerializeField]
    private float scaleMultiplier;
    [SerializeField]
    private bool useBuffer;
    [SerializeField]
    private float baseBrightness;

    private Material mat;
    // Start is called before the first frame update
    void Start()
    {
        if (Analyser.current == null) Destroy(this);
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs( Analyser.current.bandBuffer[band] )* scaleMultiplier + startScale, transform.localScale.z);
            mat.SetColor("_EmissionColor", Color.yellow * ((Analyser.current.audioBandBuffer[band] * 2)));
        
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, Analyser.current.freqBand[band] * scaleMultiplier + startScale, transform.localScale.z);
            mat.SetColor("_EmissionColor", Color.yellow * ((Analyser.current.audioBand[band] * 2)));
        }
    }
}
