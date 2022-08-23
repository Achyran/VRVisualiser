using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVector : MonoBehaviour
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
    private Transform end;
    [SerializeField]
    private float RotationSpeed;
    private enum XYZ { x,y,z }
    [SerializeField]
    private XYZ direction;
    // Start is called before the first frame update
    void Start()
    {
        if (Analyser.current == null) Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        SetDistance();
        Rotate();
    }
    private void Rotate()
    {
        switch (direction)
        {
            case XYZ.x:
                transform.Rotate(RotationSpeed, 0, 0);
                break;
            case XYZ.y:
                transform.Rotate(0, RotationSpeed, 0);
                break;
            case XYZ.z:
                transform.Rotate(0,0, RotationSpeed);
                break;
            default:
                break;
        }

    }

    private void SetDistance()
    {
        if (useBuffer)
        {
            end.localPosition = new Vector3(0, Mathf.Abs(Analyser.current.bandBuffer[band]) * scaleMultiplier + startScale, 0);
        }
        else
        {
            end.localPosition = new Vector3(0, Mathf.Abs(Analyser.current.bandBuffer[band]) * scaleMultiplier + startScale, 0);
        }
    }
}
