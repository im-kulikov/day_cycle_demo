using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    private Vector3 _mousePosition = new Vector3(255, 255, 255);
    private float _mouseSensitive = 0.25f;

    private Vector3 GetBaseInput(Vector3 position)
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            position += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            position += new Vector3(1, 0, 0);
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            position += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            position += new Vector3(0, 0, -1);
        }

        return position;
    }

    public void Update()
    {
        var curTransform = transform;

        {
            // change camera position:
            var curPosition = curTransform.position;
            var newPosition = GetBaseInput(curPosition);

            if (curPosition != newPosition)
            {
                curTransform.position = Vector3.Lerp(curPosition, newPosition, Time.deltaTime);
            }
        }

        {
            // change camera rotation
            var rotation = curTransform.eulerAngles;

            _mousePosition = Input.mousePosition - _mousePosition;
            _mousePosition = new Vector3(-_mousePosition.y * _mouseSensitive, _mousePosition.x * _mouseSensitive, 0);
            _mousePosition = new Vector3(rotation.x + _mousePosition.x, rotation.y + _mousePosition.y, 0);

            curTransform.eulerAngles = _mousePosition;
            _mousePosition = Input.mousePosition;
        }
    }
}