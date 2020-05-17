using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Horizontal Movement")]
    public float moveSpeed = 20f;
    public float moveLimitXAxis;
    public float moveLimitZAxis;

    private float _initialMoveLimitXAxis;
    private float _initialMoveLimitZAxis;

    [Header("Camera Vertical Movement")]
    public float scrollSpeed = 20f;
    public float minY = 0;
    public float maxY;

    private Vector3 _initialPosition;

    void Start() {
        _initialPosition = transform.position;
        _initialMoveLimitXAxis = moveLimitXAxis;
        _initialMoveLimitZAxis = moveLimitZAxis;
    }

    void Update()
    {
        updateHorizontally();
        updateVertically();
    }

    private void updateHorizontally() {
        Vector3 position = transform.position;
        Vector3 mousePosition = Input.mousePosition;

        if (Input.GetKey("w") || mousePosition.y >= Screen.height) {
            position.z += moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s") || mousePosition.y <= 0) {
            position.z -= moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a") || mousePosition.x <= 0) {
            position.x -= moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d") || mousePosition.x >= Screen.width) {
            position.x += moveSpeed * Time.deltaTime;
        }

        position.z = Mathf.Clamp(position.z, _initialPosition.z - moveLimitZAxis, 
            _initialPosition.z + moveLimitZAxis);
        position.x = Mathf.Clamp(position.x, _initialPosition.x - moveLimitXAxis, 
            _initialPosition.x + moveLimitXAxis);

        transform.position = position;
    }

    private void updateVertically() {
        Vector3 position = transform.position;

        position.y -= Input.mouseScrollDelta.y * scrollSpeed * 100 * Time.deltaTime;
        position.y = Mathf.Clamp(position.y, minY, maxY);

        moveLimitXAxis = _initialMoveLimitXAxis - (position.y - _initialPosition.y);
        moveLimitZAxis = _initialMoveLimitZAxis - (position.y - _initialPosition.y);

        transform.position = position;
    }
}
