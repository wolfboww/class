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
    [HideInInspector]
    public bool canMove = true;
    private bool isDoubleJump = false;

    private Transform groundCheck;
    private Transform weaponPoint;
    private Rigidbody2D rig;
    private Animator anim;

    private float masktimer = 0;
    private float maskTime = 5;
    private float checkRadius = 0.5f;
    private Vector3 Scale;
    private float scaleX;
    private int bulletIndex;

    private bool isForwardShoot = false;
    private float angletimer = 0;
    private float angleTime = 2;
    private Vector3 mousePos = Vector3.zero;
    private Vector3 mouseDir = Vector3.zero;

    void Start()
    {
        groundCheck = transform.Find("GroundCheck");
        weaponPoint = transform.Find("WeaponPoint");
        Scale = transform.localScale;
        scaleX = Scale.x;

        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveControl();
        isJump = !Physics2D.OverlapCircle(groundCheck.position, checkRadius, 1 << LayerMask.NameToLayer("Plane"));
        anim.SetBool("Stand", !isJump);
        JumpControl();


        if (IfBullet.bemask)
        {
            if (Input.GetMouseButtonDown(1))//取消伪装
                BeNotMask();

            masktimer += Time.deltaTime;
            if (masktimer >= maskTime)
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

        if (!isForwardShoot && anim.GetFloat("Edition") > 0)
        {
            angletimer += Time.deltaTime;
            if (angletimer > angleTime || Input.GetMouseButtonDown(1))
            {
                weaponPoint.localPosition = new Vector3(2f, 1.2f);
                weaponPoint.localEulerAngles = Vector3.zero;
                anim.SetFloat("Angle", 0.5f);
                isForwardShoot = true;
            }
        }

    }

    public void MoveControl()
    {
        if (!canMove)
            return;

        float H = Input.GetAxisRaw("Horizontal");
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
    }


    public void JumpControl()  //跳跃and射击
    {
        if (!isJump)
        {
            anim.ResetTrigger("Jump");
            anim.ResetTrigger("DoubleJump");
            isDoubleJump = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IfBullet.bemask)
                return;

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

    private void OnGUI()
    {
        switch (Event.current.type)
        {
            case EventType.MouseDown:
                mousePos = Event.current.mousePosition;
                break;
            case EventType.MouseDrag:
                if (anim.GetFloat("Edition") > 0)
                {
                    isForwardShoot = false;
                    angletimer = 0;

                    mouseDir = Event.current.mousePosition;
                    float angle = mouseDir.y < mousePos.y ? 0 : 1;
                    weaponPoint.localPosition = mouseDir.y < mousePos.y ?
                        new Vector3(1.5f, 2.5f) : new Vector3(1.5f, -0.2f);
                    weaponPoint.localEulerAngles =
                        new Vector3(0, 0, 45) * (mouseDir.y < mousePos.y ? 1 : -1);
                    anim.SetFloat("Angle", angle);

                }
                break;
        }
    }

    public void BeNotMask()
    {
        GameController.Instance.Mask(null);
        masktimer = 0;
        IfBullet.bemask = false;
        if (transform.parent != null)
            transform.SetParent(null);
    }
}
