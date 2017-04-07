using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Enemy0,
        Enemy1,
        Enemy2,


    }
    public int hp = 1;
    public float speed = 2;
    public  int score = 100;
    public EnemyType type = EnemyType.Enemy0;
    public  bool isDeath = false;
    public Sprite[] explosionSprites;
    private float timer = 0;
    public int explosingAnimationFrame = 10;
    private SpriteRenderer render;
    public float hitTimer = 0.2f;
    private float resetHitTime;
    public Sprite[] hitSprites;
  
    void Start()
    {
        render = this.GetComponent<SpriteRenderer>();

        resetHitTime = hitTimer;
        hitTimer = 0;
    }

    // Update is called once per frame
    void Update( )
    {  
        this.transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (this.transform.position.y <= -5.6)
        {
            Destroy(this.gameObject);
        }
        if (isDeath)
        {

            timer += Time.deltaTime;
            int frameIndex = (int)(timer / (1f / explosingAnimationFrame));//强制转换INT
            if (frameIndex >= explosionSprites.Length)
            {
                Destroy(this.gameObject);
            }
            else
                render.sprite = explosionSprites[frameIndex];
        }
        else
        {
            if (type == EnemyType.Enemy1 || type == EnemyType.Enemy2)
            {
                if (hitTimer >= 0)
                {
                    hitTimer -= Time.deltaTime;
                    int frameIndex = (int)((resetHitTime - hitTimer) / (1f / explosingAnimationFrame));
                    frameIndex %= 2;//只有2个动画
                    render.sprite = hitSprites[frameIndex];
                }
            }
        }
        
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && BombManager._instance.count > 0)
        {//只有炸弹数目大于0才会被调用
            toDie();//diaoyong fangfa
            
        }

    }
    public void BeHit()
    {

        //爆炸
        hp -= 1;//每次击中血量扣1
        if (hp <= 0)
        {
            toDie();
        }
        else
        {
            hitTimer = resetHitTime;
        }
    }
    public  void toDie()
    {
        if (!isDeath)
        {
            isDeath = true;
            this.GetComponent<AudioSource>().Play();
            
            GameManager._instance.Score += score;
        }
    }
  
}
