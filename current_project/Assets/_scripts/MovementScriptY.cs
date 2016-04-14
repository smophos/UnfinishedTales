using UnityEngine;
using System.Collections;

public class MovementScriptY : MonoBehaviour {

    Vector3 floatY;
    float originalY;

    public float floatStrength = 2;

    void Start()
    {

        this.originalY = this.transform.position.y;
    }

    void Update()
    {
        floatY = transform.position;
        floatY.y = originalY + (Mathf.Sin(Time.time) * floatStrength);
        this.transform.position = floatY;
    }
}
