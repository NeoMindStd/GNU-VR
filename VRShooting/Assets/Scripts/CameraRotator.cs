using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] float angularVelocity = 30f;
    float horizontalAngle = 0f;
    float verticalAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var horizontalRotation = Input.GetAxis("Horizontal") * angularVelocity * Time.deltaTime;
        // -: 상하 제어 반전
        var verticalRotation = -Input.GetAxis("Vertical") * angularVelocity * Time.deltaTime;

        horizontalAngle += horizontalRotation;
        verticalAngle += verticalRotation;

        verticalAngle = Mathf.Clamp(verticalAngle, -80f, 80f);

        transform.rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0f);
    }
}
