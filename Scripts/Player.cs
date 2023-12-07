using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    GameObject currentFloor;
    [SerializeField] int Hp;
    [SerializeField] Slider HpBar;
    [SerializeField] Text ScoreText;
    int score;
    float scoreTime;
    Animator anim;
    SpriteRenderer render;
    AudioSource deathSound;
    [SerializeField] GameObject replayButton;
    // Start is called before the first frame update
    void Start()
    {
        Hp = 100;
        score = 0;
        scoreTime = 0f;
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        deathSound = GetComponent<AudioSource>();
    }

    void Update()// Update is called once per frame
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(moveSpeed*Time.deltaTime, 0, 0);
            render.flipX = false;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-moveSpeed*Time.deltaTime, 0, 0);
            render.flipX = true;
        }
        UpdateScore();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Normal")
        {
            if(other.contacts[0].normal == new Vector2(0f, 1f))
            {
                currentFloor = other.gameObject;
                other.gameObject.GetComponent<AudioSource>().Play();
            }
            
        }
        else if(other.gameObject.tag == "Poison")
        {
            if(other.contacts[0].normal == new Vector2(0f, 1f))
            {
                currentFloor = other.gameObject;
                ModifyHp(-5);
                anim.SetTrigger("Hurt");
                other.gameObject.GetComponent<AudioSource>().Play();
            }
            
        }
        else if(other.gameObject.tag == "Celling")
        {
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
            ModifyHp(-10);
            anim.SetTrigger("Hurt");
            other.gameObject.GetComponent<AudioSource>().Play();
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "DeathLine")
        {
            ModifyHp(-100);
        }
    }

    void ModifyHp(int num)
    {
        Hp += num;
        if(Hp>100)
        {
            Hp = 100;
        }
        else if(Hp<=0)
        {
            Hp = 0;
            Die();
        }
        HpBar.value = Hp;
    }

    void UpdateScore()
    {
        scoreTime += Time.deltaTime;
        if(scoreTime>2f)
        {
            score++;
            scoreTime = 0f;
            ScoreText.text = "Score:" + score.ToString();
        }
    }
    void Die()
    {
        deathSound.Play();
        Time.timeScale = 0f;
        replayButton.SetActive(true);
    }
    public void Replay()
    {
        Time.timeScale=1f;
        SceneManager.LoadScene("SampleScene");
    }
}
