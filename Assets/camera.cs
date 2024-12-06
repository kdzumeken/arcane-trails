using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    private float turnSpeed = 4.0f;
    private Transform posHero;
    private Vector3 offset;

    // Variables for vertical rotation limits
    private float verticalRotation = 0f;
    private float verticalRotationLimit = 45f;

    void Start()
    {
        posHero = GameObject.Find("wizard").transform.Find("head").transform;
        offset = new Vector3(posHero.localPosition.x + 1f, posHero.localPosition.y - 1f, posHero.localPosition.z - 3.2f);
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal rotation
        float horizontalRotation = Input.GetAxis("Mouse X") * turnSpeed;
        offset = Quaternion.AngleAxis(horizontalRotation, Vector3.up) * offset;

        // Vertical rotation with limits
        float verticalInput = -Input.GetAxis("Mouse Y") * turnSpeed;
        verticalRotation = Mathf.Clamp(verticalRotation + verticalInput, -verticalRotationLimit, verticalRotationLimit);
        Quaternion verticalQuaternion = Quaternion.AngleAxis(verticalRotation, Vector3.right);

        // Apply rotations
        Vector3 rotatedOffset = verticalQuaternion * offset;
        transform.position = posHero.position + rotatedOffset;
        transform.LookAt(posHero.position);
    }
}
