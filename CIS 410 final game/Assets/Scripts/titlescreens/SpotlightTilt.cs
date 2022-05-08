using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightTilt : MonoBehaviour
{
    public Transform LightTransform;
    public Transform from;
    public Transform to;
    public float rotationSpeed;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = (Time.time * rotationSpeed) % 360f;
        float lerp_amount = (Mathf.Sin(angle) + 1.0f) / 2.0f;
        transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, lerp_amount);
    }
}
