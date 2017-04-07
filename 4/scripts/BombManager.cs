using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public GameObject bomb;
    public GUIText bombNumber;


    public int  count;

    public static BombManager _instance;
    void Awake()
    {
        _instance = this;
        bomb.SetActive(false);
        bombNumber.gameObject.SetActive(false);
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0) && BombManager._instance.count > 0)
        {
            this.UseABomb();
        }
        
    }
     
    public void AddAbomb()
    {
        bomb.SetActive(true);
        bombNumber.gameObject.SetActive(true);
        count++;
        bombNumber.text = "X" + count;

    }
    public void UseABomb()
    {   
        if (count > 0)
          {
            this.GetComponent<AudioSource>().Play();
            count--;
            bombNumber.text = "X" + count;
            if (count <= 0)
            {
                bomb.SetActive(false);
                bombNumber.gameObject.SetActive(false);

            }

        }
    }
}
