using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    float walkSpeed = 5;
    [SerializeField]
    float runSpeed = 10;
    [SerializeField]
    float jumpForce = 5;
    
    PlayerController playerController;
    Rigidbody playerRigidbody;
    Animator playerAnimator;
    public GameObject followTransform;

    Vector2 inputVector = Vector2.zero;
    Vector3 moveDirection = Vector3.zero;
    Vector2 lookInput = Vector2.zero;

    public float aimSensativity = 1;

    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isJumpingHash = Animator.StringToHash("isJumping");
    public readonly int isRunningHash = Animator.StringToHash("isRunning");
    public readonly int verticalAimHash = Animator.StringToHash("VerticalAim");

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.instance.cursorActive)
        {
            AppEvents.InvokeMouseCursorEnable(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        followTransform.transform.rotation *= Quaternion.AngleAxis(lookInput.x * aimSensativity, Vector3.up);
        
        followTransform.transform.rotation *= Quaternion.AngleAxis(lookInput.y * aimSensativity, Vector3.left);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;

        
        float min = -60;
        float max = 70.0f;
        float range = max - min;
        float offsetToZero = 0 - min;
        float aimAngle = followTransform.transform.localEulerAngles.x;
        aimAngle = (aimAngle > 180) ? aimAngle - 360 : aimAngle;
        float val = (aimAngle + offsetToZero) / (range);
        print(val);
        playerAnimator.SetFloat(verticalAimHash, val);
        //if (angle > 180 && angle < min)
        //{
        //    angles.x = min;
        //}
        //else if (angle < 180 && angle > max)
        //{
        //    angles.x = max;
        //}
        if (angle > 180 && angle < 300)
        {
            angles.x = 300;
        }
        else if (angle < 180 && angle > 70)
        {
            angles.x = 70;
        }

        followTransform.transform.localEulerAngles = angles;
        transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);

        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);


        if (playerController.isJumping)
        {
            return;
        }
        if(!(inputVector.magnitude > 0))
        {
            moveDirection = Vector3.zero;
        }

        moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;
        float currentSpeed = playerController.isRunning ? runSpeed : walkSpeed;

        Vector3 movementDirection = moveDirection * (currentSpeed * Time.deltaTime);

        transform.position += movementDirection;

    }

    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
        playerAnimator.SetFloat(movementXHash, inputVector.x);
        playerAnimator.SetFloat(movementYHash, inputVector.y);
    }

    public void OnRun(InputValue value)
    {
        playerController.isRunning = value.isPressed;
        playerAnimator.SetBool(isRunningHash, playerController.isRunning);
    }

    public void OnJump(InputValue value)
    {
        if (playerController.isJumping)
        {
            return;
        }
        playerController.isJumping = true;
 
        playerRigidbody.AddForce((transform.up + moveDirection) * jumpForce, ForceMode.Impulse);
        playerAnimator.SetBool(isJumpingHash, playerController.isJumping);
    }
    public void OnLookingAround(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
  
    public void OnAim(InputValue value)
    {
        playerController.isAiming = value.isPressed;
    }

    public void OnQuitGame(InputValue value)
    {
        if (value.isPressed)
        {
            Application.Quit();
        }
    }
    private bool IsGroundCollision(ContactPoint[] contacts)
    {
        for (int i = 0; i < contacts.Length; i++)
        {
            if(1 - contacts[i].normal.y < 1f)
            {
                return true;
            }
        }
        return false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground") && !playerController.isJumping) return;

        if (IsGroundCollision(collision.contacts))
        {
            playerController.isJumping = false;
            playerAnimator.SetBool(isJumpingHash, false);
        }

        playerController.isJumping = false;
        playerAnimator.SetBool(isJumpingHash, false);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground") && !playerController.isJumping || playerRigidbody.velocity.y > 0) return;

        if (IsGroundCollision(collision.contacts))
        {
            playerController.isJumping = false;
            playerAnimator.SetBool(isJumpingHash, false);
        }

        playerController.isJumping = false;
        playerAnimator.SetBool(isJumpingHash, false);
    }

}
