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
    public bool virtual3D = false;
    public static bool canShoot = true;
    private bool isDoubleJump = false;

    private Transform groundCheck;
    private Transform weaponPoint;
    private Transform bubble;
    private Rigidbody2D rig2d;
    private Animator anim;
    private Coroutine bubbleCor = null;

    private float masktimer = 0;
    private float maskTime = 10;
    private float checkRadius = 0.5f;
    private Vector3 Scale;
    private float scaleX;
    private int bulletIndex;

    private bool isForwardShoot = false;
    private float angletimer = 0;
    private float angleTime = 1;
    private Vector3 mousePos = Vector3.zero;
    private Vector3 mouseDir = Vector3.zero;

    private Vector3 offset2d;
    private Vector3 size2d;


    void Start()
    {
        groundCheck = transform.Find("GroundCheck");
        weaponPoint = transform.Find("WeaponPoint");
        bubble = transform.Find("BubblePos");
        Scale = transform.localScale;
        scaleX = Scale.x;

        if (GetComponent<Rigidbody2D>())
            rig2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        offset2d = GetComponent<BoxCollider2D>().offset;
        size2d = GetComponent<BoxCollider2D>().size;
    }

    // Update is called once per frame
    void Update()
    {
        MoveControl();
        if (!virtual3D)
            isJump = !Physics2D.OverlapCircle(groundCheck.position, checkRadius, 1 << LayerMask.NameToLayer("Plane"));
        else
            isJump = Physics.OverlapBox(groundCheck.position, Vector3.one * checkRadius, Quaternion.identity, 1 << 8).Length.Equals(0);
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

        if (anim.GetFloat("Edition") > 0)
        {
            transform.Find("Highlight").gameObject.SetActive(false);
            if (!isForwardShoot)
            {
                angletimer += Time.deltaTime;
                if (angletimer > angleTime /*|| Input.GetMouseButtonDown(1)*/)
                {
                    weaponPoint.localPosition = new Vector3(2f, 1.2f);
                    weaponPoint.localEulerAngles = Vector3.zero;
                    anim.SetFloat("Angle", 0.5f);
                    isForwardShoot = true;
                }
            }
        }

        if (virtual3D)
        {
            if (GetComponent<BoxCollider2D>())
                Destroy(GetComponent<BoxCollider2D>());
            if (GetComponent<Rigidbody2D>())
                Destroy(GetComponent<Rigidbody2D>());
            if (!GetComponent<BoxCollider>())
                gameObject.AddComponent<BoxCollider>();
            if (!GetComponent<Rigidbody>())
            {
                gameObject.AddComponent<Rigidbody>();
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
        else
        {
            if (GetComponent<BoxCollider>())
                Destroy(GetComponent<BoxCollider>());
            if (GetComponent<Rigidbody>())
                Destroy(GetComponent<Rigidbody>());
            if (!GetComponent<BoxCollider2D>())
            {
                gameObject.AddComponent<BoxCollider2D>();
                GetComponent<BoxCollider2D>().offset = offset2d;
                GetComponent<BoxCollider2D>().size = size2d;
            }
            if (!GetComponent<Rigidbody2D>())
            {
                gameObject.AddComponent<Rigidbody2D>();
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }


        if (bubble.gameObject.activeInHierarchy)
        {
            if (bubbleCor != null)
                return;
            bubbleCor = StartCoroutine(Bubble());
        }
        else
        {
            StopCoroutine(Bubble());
            bubbleCor = null;
        }
    }

    IEnumerator Bubble()
    {
        while (bubble.gameObject.activeInHierarchy)
        {
            bubble.GetComponentInChildren<AudioSource>().enabled = true;
            yield return new WaitForSeconds(bubble.GetComponentInChildren<AudioSource>().clip.length + 0.5f);
            bubble.GetComponentInChildren<AudioSource>().enabled = false;
            yield return new WaitForSeconds(1);
        }
    }

    public void MoveControl()
    {
        if (!canMove)
            return;

        float h = Input.GetAxisRaw("Horizontal");
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerDead"))
            anim.SetFloat("Speed", h);
        else
            h = 0;

        if (!h.Equals(0))
        {
            Scale.x = (h > 0 ? 1 : -1) * scaleX;
            transform.localScale = Scale;
            if (virtual3D)
            {
                float v = Input.GetAxisRaw("Vertical");
                transform.Translate(new Vector3(h * Time.deltaTime * moveSpeed, 0f, v * Time.deltaTime * moveSpeed));
            }
            else
                transform.Translate(new Vector2(h, 0) * Time.deltaTime * moveSpeed);
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
                if (!virtual3D)
                    rig2d.velocity = new Vector2(rig2d.velocity.x, jumpForce);
                else
                    GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, jumpForce, GetComponent<Rigidbody>().velocity.z);
            }
            else if (!isDoubleJump)
            {
                isDoubleJump = true;
                anim.SetTrigger("DoubleJump");
                if (!virtual3D)
                    rig2d.velocity = new Vector2(rig2d.velocity.x, jumpForce);
                else
                    GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, jumpForce, GetComponent<Rigidbody>().velocity.z);
            }

        }

        if (Input.GetMouseButtonDown(0) && canShoot)    // 射击and跳射
        {
            if (anim.GetBool("GetGun") && !IfBullet.bemask)
            {
                anim.SetFloat("Shoot", 1);
                GetComponent<AnimatorController>().Shoot();
            }
        }
        else if (Input.GetMouseButtonUp(0))
            anim.SetFloat("Shoot", 0);

        if (Input.GetMouseButtonDown(1) && anim.GetFloat("Edition") > 1.9f)     //道具
        {
            if (anim.GetBool("GetGun") && !IfBullet.bemask)
            {
                anim.SetFloat("Prop", 1);
                weaponPoint.Find("LightFX").gameObject.SetActive(true);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            anim.SetFloat("Prop", 0);
            weaponPoint.Find("LightFX").gameObject.SetActive(false);
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
