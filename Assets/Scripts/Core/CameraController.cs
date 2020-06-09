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
    public float minY = 0f;
    public float maxY = 20f;
    public float characterHeightOffset = 40f;

    private Vector3 _initialPosition;
    public int cameraAngle1;
    public int cameraAngle2;

    void Awake() {
        _initialPosition = transform.position;
        _initialMoveLimitXAxis = moveLimitXAxis;
        _initialMoveLimitZAxis = moveLimitZAxis;
    }

    void Start() {
        
    }

    void Update()
    {
        updateHorizontally();
        updateVertically();
    }

    // Set up the camera's initial location based on the position of character
    public void setupCamera(Vector3 position) {
        position.y += characterHeightOffset;
        position.z -= characterHeightOffset/2;

        float height = maxY - minY;
        maxY = position.y;
        minY = maxY - height;

        gameObject.transform.position = position;

        _initialPosition = position;
        moveLimitXAxis = _initialMoveLimitXAxis;
        moveLimitZAxis = _initialMoveLimitZAxis;
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

    public void angleSwtich()
    {
        Vector3 angle = transform.rotation.eulerAngles;

        if ((int)angle.x == cameraAngle1)
        {
            Quaternion rotate = new Quaternion();
            angle.x = cameraAngle2;
            rotate.eulerAngles = angle;
            transform.rotation = rotate;
            return;
        }

        if((int)angle.x == cameraAngle2)
        {
            Quaternion rotate = new Quaternion();
            angle.x = cameraAngle1;
            rotate.eulerAngles = angle;
            transform.rotation = rotate;
            return;
        }
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
