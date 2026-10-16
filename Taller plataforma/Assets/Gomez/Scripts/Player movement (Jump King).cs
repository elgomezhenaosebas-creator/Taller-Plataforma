using Unity.VisualScripting;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float walkSpeed;
    private float moveInput;
    public bool isGrounded;
    private Rigidbody2D rb;
    public LayerMask groundMask;
    public PhysicsMaterial2D bounceMat, normalMat;
    public bool canJump = true;
    public float jumpValue = 0.0f;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

    }


    void Update()
    {
        moveInput = Input.GetAxisRaw("horizontal");

        if (jumpValue == 0.0f && isGrounded)
        {

        }

        rb.linearVelocity = new Vector2(moveInput * walkSpeed, rb.linearVelocity.y);

        isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f),
        new Vector2(0.9f, 0.4f), 0f, groundMask);

        if (jumpValue > 0)
        {
            rb.sharedMaterial = bounceMat;
        }else
        {
            rb.sharedMaterial = normalMat;
        }


        if (Input.GetKey("space") && isGrounded && canJump)
        {
            jumpValue += 0.1f;
        }

        if (Input.GetKeyDown("space") && isGrounded && canJump)

        if (jumpValue >= 20f && isGrounded)
        {
            float tempx = moveInput * walkSpeed;
            float tempy = jumpValue;
            rb.linearVelocity = new Vector2(tempx, tempy);
            Invoke("ResetJump", 0.2f);
        }

        if (Input.GetKeyUp("space"))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(moveInput * walkSpeed, jumpValue);
                jumpValue = 0.0f;
            }
            canJump = true;
        }



    }
    void ResetJump()
    {
        canJump = false;
        jumpValue = 0;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f), new Vector2(0.9f, 0.2f));
    }
}