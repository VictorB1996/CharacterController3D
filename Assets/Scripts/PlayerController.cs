using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = .4f;
    public LayerMask groundMaskCheck;
    private bool IsGrounded;
    private Vector3 velocity;

    [SerializeField] private Transform mainCamera;

    [SerializeField] private Animator characterAnimator;

    [SerializeField] private float turnSmoothTime = 0.1f;

    float turnSmoothVelocity;

    [SerializeField] private bool canPickUp;
    [SerializeField] private bool hasItem;
    GameObject ObjectToPickUp;
    [SerializeField] private Transform characterRightHand;
    [SerializeField] private Vector3 weaponPositionOffset;
    [SerializeField] private Vector3 weaponRotationOffset;

    private Weapon currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        canPickUp = true;
        hasItem = false;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // Tried with characterController.IsGrounded as well but did not work as expected
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMaskCheck);

        if (IsGrounded && velocity.y < 0)
            velocity.y = -2f;

        if(direction.magnitude >= 0.1)
        {
            if (!characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pick Up"))
            {
                float angleToPoint = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angleToPoint, ref turnSmoothVelocity, turnSmoothTime);

                transform.rotation = Quaternion.Euler(0, angle, 0);

                Vector3 moveDirection = Quaternion.Euler(0f, angleToPoint, 0f) * Vector3.forward;

                characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
            }

        }
        characterAnimator.SetFloat("Speed", direction.magnitude, .25f, Time.deltaTime);

        if (IsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        characterAnimator.SetBool("IsGrounded", IsGrounded);

        PickUp();
        Attack(currentWeapon);
        DropWeapon();
    }

    private void Attack(Weapon weapon)
    {
        if(hasItem)
        {
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                weapon.Attack();
            }
        }
    }

    private void PickUp()
    {
        if(canPickUp && !hasItem)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                ObjectToPickUp.GetComponent<Rigidbody>().isKinematic = true;
                characterAnimator.SetTrigger("PickUp");
                Invoke("PickUpWeapon", 1);
            }
        }
    }

    private void PickUpWeapon()
    {
        ObjectToPickUp.transform.parent = characterRightHand;
        ObjectToPickUp.transform.localPosition = weaponPositionOffset;
        ObjectToPickUp.transform.localRotation = Quaternion.Euler(weaponRotationOffset);

        hasItem = true;
        canPickUp = false;
    }

    private void DropWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            hasItem = false;
            canPickUp = true;

            ObjectToPickUp.transform.parent = null;
            ObjectToPickUp.GetComponent<Rigidbody>().isKinematic = false;
            ObjectToPickUp = null;
            //currentWeapon = null;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Weapon")
        {
            canPickUp = true;
            currentWeapon = hit.gameObject.GetComponent<Weapon>();
            ObjectToPickUp = hit.gameObject;
        }
    }
    
}
