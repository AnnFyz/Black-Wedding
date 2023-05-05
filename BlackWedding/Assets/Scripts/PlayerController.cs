using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;
    private Vector3 _input;
    public static bool IsPaused = false;
    public bool paused = IsPaused;
    [SerializeField] Animator animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Time.timeScale = 1;
    }
    private void Update()
    {
          paused = IsPaused;;
        if (!IsPaused)
        {
            GatherInput();
            Look();
        }

    }

    private void FixedUpdate()
    {
        if (!IsPaused)
        {
            Move();
        }       
    }

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        //_rb.MovePosition(transform.position + transform.forward * _input.normalized.magnitude * _speed * Time.fixedDeltaTime);
        if (_input.x  > 0 || _input.x < 0 || _input.z > 0 || _input.z < 0)
        {
            _rb.velocity = transform.forward * _speed * Time.fixedDeltaTime;
            animator.SetBool("IsWalking", _rb.velocity.magnitude > 0.01f);
        }
        else
        {
            _rb.velocity = new Vector3(0, 0, 0);
            animator.SetBool("IsWalking",false);
        }
       
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
