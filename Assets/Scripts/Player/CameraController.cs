using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform Head;

    private float _rotationSpeedHorizontal = 5f;
    private float _rotationSpeedVertical = 5f;

    private float _minVertical = -45f;
    private float _maxVertical = 45f;

    private float _rotateX = 0f;

    private bool _lock = false;

    private void MoveCamera()
    {
        _rotateX -= Input.GetAxis("Mouse Y") * _rotationSpeedVertical;
        _rotateX = Mathf.Clamp(_rotateX, _minVertical, _maxVertical);

        float deltaX = Input.GetAxis("Mouse X") * _rotationSpeedHorizontal;
        float _rotateY = Head.transform.localEulerAngles.y + deltaX;

        Head.transform.localEulerAngles = new Vector3(0, _rotateY, -_rotateX);
    }

    void Update()
    {
        if (!_lock)
        {
            MoveCamera();
        }
    }

    public void LockCamera(bool state)
    {
        _lock = state;
    }
}