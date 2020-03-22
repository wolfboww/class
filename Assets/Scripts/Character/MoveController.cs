using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float moveSpeed = 2;
    public float jumpForce = 200;
    public float bounceForce = 1;
    public List<GameObject> bullets;

    [HideInInspector]
    public bool isJump = false;
    private bool isDoubleJump = false;

    private Transform groundCheck;
    private Rigidbody2D rig;
    private Animator anim;

    private float time = 0;
    private float maskTime = 5;
    private float checkRadius = 0.5f;
    private Vector3 Scale;
    private float scaleX;
    private int bulletIndex;
    void Start()
    {
        groundCheck = transform.Find("GroundCheck");
        Scale = transform.localScale;
        scaleX = Scale.x;

        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float H = Input.GetAxis("Horizontal");
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerDead"))
            anim.SetFloat("Speed", H);
        else
            H = 0;

        if (!H.Equals(0))
        {
            Scale.x = (H > 0 ? 1 : -1) * scaleX;
            transform.localScale = Scale;
            transform.Translate(new Vector2(H, 0) * Time.deltaTime * moveSpeed);
        }

        isJump = !Physics2D.OverlapCircle(groundCheck.position, checkRadius, 1 << LayerMask.NameToLayer("Plane"));
        anim.SetBool("Stand", !isJump);
        JumpController();


        if (IfBullet.bemask)
        {
            if (Input.GetMouseButtonDown(1))//取消伪装
                BeNotMask();

            time += Time.deltaTime;
            if (time >= maskTime)
                BeNotMask();
        }

        if (!Input.GetAxis("Mouse ScrollWheel").Equals(0))
        {
            bulletIndex = Input.GetAxis("Mouse ScrollWheel") > 0 ? bulletIndex + 1 : bulletIndex - 1;
            if (bulletIndex > bullets.Count - 1)
                bulletIndex = 0;
            if (bulletIndex < 0)
                bulletIndex = bullets.Count - 1;
        }
        if (bullets.Count != 0)
            GetComponent<AnimatorController>().bullet = bullets[bulletIndex];
    }

    public void JumpController()  //跳跃and射击
    {
        if (!isJump)
        {
            anim.ResetTrigger("Jump");
            anim.ResetTrigger("DoubleJump");
            isDoubleJump = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");
            if (!isJump)
            {
                isJump = true;
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
            }
            else if (!isDoubleJump)
            {
                isDoubleJump = true;
                anim.SetTrigger("DoubleJump");
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
            }
        }

        if (Input.GetMouseButtonDown(0))    // 射击and跳射
        {
            if (anim.GetBool("GetGun") && !IfBullet.bemask)
            {
                anim.SetFloat("Shoot", 1);
                gameObject.GetComponent<AnimatorController>().Shoot();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            anim.SetFloat("Shoot", 0);
        }
    }

    public void BeNotMask()
    {
        GameController.Instance.Mask(false, null);
        time = 0;
        IfBullet.bemask = false;
        if (transform.parent != null)
            transform.SetParent(null);
    }
}
