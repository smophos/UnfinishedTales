using UnityEngine;
using System.Collections;

public class MovementScriptX : MonoBehaviour {

    Vector3 floatX;
    float originalX;

    public float floatStrength = 2;

    void Start()
    {

        this.originalX = this.transform.position.x;
    }

    void Update()
    {
        floatX = transform.position;
        floatX.x = originalX + (Mathf.Sin(Time.time) * floatStrength);
        this.transform.position = floatX;
    }
}
