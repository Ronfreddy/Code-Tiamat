using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    private Vector3 _movementDirection;
    private Vector3 _movementDirectionX;
    private Vector3 _movementDirectionY;
    public float _movementSpeed = 5f;
    public Rigidbody2D rb;


    private void Update()
    {
        if (!PlayerInputController.Instance.canInput)
        { 
            _movementDirectionX = Vector3.zero;
            _movementDirectionY = Vector3.zero;
            return;
        }
        if(Input.GetKey(KeyCode.W))
        {
            _movementDirectionY = Vector3.up;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            _movementDirectionY = Vector3.down;
        }
        else
        {
            _movementDirectionY = Vector3.zero;
        }

        if(Input.GetKey(KeyCode.A))
        {
            _movementDirectionX = Vector3.left;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            _movementDirectionX = Vector3.right;
        }
        else
        {
            _movementDirectionX = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        _movementDirection = (_movementDirectionX + _movementDirectionY).normalized;
        rb.linearVelocity = _movementDirection * _movementSpeed;
    }
}
