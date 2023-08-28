using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialLerper : MonoBehaviour
{
    public Material materialToLerp;
    
    public float lerpSpeed = 1.0f;
    public Gradient colors;
    public bool repeatable = true;
    private float _startTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!repeatable)
        {
            float t = (Time.time - _startTime) * lerpSpeed;
            materialToLerp.color = colors.Evaluate(t);
            materialToLerp.SetColor("_EmissionColor", colors.Evaluate(t));
        }
        else
        {
            float t = (Mathf.Sin(Time.time - _startTime) * lerpSpeed);
            materialToLerp.color = colors.Evaluate(t);
            materialToLerp.SetColor("_EmissionColor", colors.Evaluate(t));
        }
    }
}
