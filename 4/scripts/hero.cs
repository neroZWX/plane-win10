using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hero : MonoBehaviour
{
    public bool Animation = true;
    public int frameCountPerSeconds = 10;
    public float speed = 4F;
    public float timer = 0;
    public Sprite[] sprites;
    public float superGunTime = 10f;
    public Gun gunTop;
    public Gun gunLeft;
    public Gun gunRight;
    private float resetsuperGunTime;
    private SpriteRenderer spriteRender;
    private bool isMouseDown = false;
    private Vector3 lastMousePosition = Vector3.zero;
    private Vector3 LastToughPosition = Vector3.zero;
    private Transform Hero;
    private int gunCount = 1;

    private void Start()
    {
        spriteRender = this.GetComponent<SpriteRenderer>();
        Hero = GameObject.FindGameObjectWithTag("Player").transform;
        resetsuperGunTime = superGunTime;
        superGunTime = 0;
    }



    void Update()
    {if ( GameManager._instance.gamestate == Gamestate.Running)
        {
            //float acceleration = 4.0f;
            transform.Translate(Input.acceleration.x / 3, Input.acceleration.y / 3, 0);
            checkPosition();
           LastToughPosition= Vector3.zero;
        }
        if (Animation)
        {
            timer += Time.deltaTime;
            int frameIndex = (int)(timer / (1f / frameCountPerSeconds));//当前第几帧。
            int frame = frameIndex % 2;//运行几个图片
            this.GetComponent<SpriteRenderer>().sprite = sprites[frame];
        }
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            lastMousePosition = Vector3.zero;
        }
        if (isMouseDown && GameManager._instance.gamestate == Gamestate.Running)
        {//只有运行游戏时才能移动主角
            if (lastMousePosition != Vector3.zero)
            {
                Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
                transform.position = transform.position + offset;
                checkPosition();
            }
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        superGunTime -= Time.deltaTime;
        if (superGunTime > 0)
        {
            if (gunCount == 1)
            {
                transformToSuperGun();
            }
        }
        else
        {
            if (gunCount == 3)
            {
                transformToNormalGun();

            }
        }
    }
    private void transformToSuperGun()
    {
        gunCount = 3;
        gunLeft.openFire();
        gunRight.openFire();
        gunTop.stopFire();
    }
    private void transformToNormalGun()
    {
        gunCount = 1;
        gunLeft.stopFire();
        gunRight.stopFire();
        gunTop.openFire();
    }

    private void checkPosition()
    {
        Vector3 p = transform.position;
        float x = p.x;
        float y = p.y;
        if (x < -2.0f)
        {
            x = -2.0f;
        }
        if (x > 2.0f)
        {
            x = 2.0f;
        }
        if (y < -3.625f)
        {
            y = -3.625f;
        }
        if (y > 3.4f)
        {
            y = 3.4f;
        }
        transform.position = new Vector3(x, y, 0);
    }
    //public AudioClip out_porp;
    public void OnTriggerEnter2D(Collider2D collider)//撞到奖励物品
    {

        if (collider.tag == "Award")
        {
            this.GetComponent<AudioSource>().Play();

            //AudioSource.PlayClipAtPoint(out_porp, transform.localPosition);

            Award award = collider.GetComponent<Award>();
            if (award.type == 0)
            {
                

                //换子弹
                superGunTime = resetsuperGunTime;
                Destroy(collider.gameObject);
            }
            else if (award.type == 1)
            {
                BombManager._instance.AddAbomb();
                Destroy(collider.gameObject);

            }
        }
        else if (collider.tag == "Enemy") {
            this.GetComponent<AudioSource>().Play();
            Destroy(this.gameObject);
            GameOver._instance.Show(GameManager._instance.Score );
        }
    }
}

