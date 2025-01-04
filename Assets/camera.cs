using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_camera : MonoBehaviour
{
    private float turnSpeed = 4.0f;
    private Transform posHero;
    private Vector3 offset;
    void Start()
    {
        posHero = GameObject.Find("wizard").transform.Find("char_point_cam").transform;
        offset = new Vector3(posHero.localPosition.x + 1f, posHero.localPosition.y - 1f, posHero.localPosition.z - 3.2f);
    }

    // Update is called once per frame
    void Update()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        transform.position = posHero.position + offset;
        transform.LookAt(posHero.position);
    }
}
