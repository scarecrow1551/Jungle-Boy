using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerScript : MonoBehaviour
{
    [Header("Базовые настройки")]
    public float jump_Impulse = 20f;
    public GameObject sfx_attack; // префаб эффекта при ударе
    // приватные но базовые настройки
    private float base_Damage;
    private float animation_speed;
    private float baseHealth;

    [Header("Прочие настройки")]
    private float health;
    //==========================//
    //        Компоненты        //
    //==========================//
    public bool is_mobile;
    public Rigidbody2D rigidbody;
    private CapsuleCollider2D capsule_collider;
    private Animator animation;
    private Vector2 child_box_size;
    private GameManager game_Manager;
    //=========================//
    //          Атака          //
    //=========================//
    private float attack_time_base;
    private float attack_time = 0f;
    private int   attack_set = 0;
    private bool  isAttacking = false;
    private bool  isAlive = true;

    private bool  is_attack_check = false;
    private bool  sfx_check;
    private bool  is_faded;
    //=====================//
    //     Полоса жизни    //
    //=====================//
    private Image healthBar;
    private Image healthBar2;
    private float fadeTime_base = 255f;
    private float fadeTime;
    //=============================//
    //   Статитические переменные  //
    //=============================//
    public static bool allPause = false;
    public static bool isDeath  = false;

    private bool pause = false;

    //=============================//
    //       Прочие перемнные      //
    //=============================//
    public static float speed;
    void Start()
    {
        animation = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        capsule_collider = GetComponent<CapsuleCollider2D>();
        child_box_size = transform.GetChild(0).GetComponent<BoxCollider2D>().size;
        game_Manager = FindObjectOfType<GameManager>();

        //    настройка локальных переменных
        base_Damage     = PlayerPrefsInit.attack;
        animation_speed = PlayerPrefsInit.attack_speed;
        baseHealth      = PlayerPrefsInit.health;

        //=========================
        health = baseHealth;
        healthBar  = transform.GetChild(2).transform.GetChild(1).GetComponent<Image>(); // red
        healthBar2 = transform.GetChild(2).transform.GetChild(0).GetComponent<Image>(); // black


        //=========================
        isDeath = false;

        allPause = false;

        if (!is_mobile)
            speed = 5f;

        UpdateAnimClipTimes();
    }



    public void JumpMethod(float impulse)
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(new Vector2(0, impulse), ForceMode2D.Impulse);

    }


    public void TakeDamage(float takeDamage)
    {
        health = health - takeDamage;
        if (health <= 0f)
        {
            isDeath = true;
            FreezePlayer(true);
            animation.SetInteger("anim", 3); // death anim
            game_Manager.End_Game();
        }
        else
        {
            healthBar.fillAmount = (health / baseHealth);
            fadeTime = fadeTime_base;
            if (fadeTime > 0f && !is_faded)
            {
                is_faded = true;

                StartCoroutine(fade_time());
            }
        }
    }

    private void Moving()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float speedX = speed * Input.GetAxis("Horizontal");
        if (is_mobile)
            speedX = speed;
        //~~~~~~~~~~Moving~~~~~~~~~~// 
        rigidbody.velocity = new Vector2(speedX, rigidbody.velocity.y);

        //~~~~~~~~~Animation~~~~~~~~//
        bool groundCheck = Physics2D.OverlapBox(transform.GetChild(0).position, child_box_size, 90f, 1 << LayerMask.NameToLayer("ground"));

        if (!groundCheck)
        {
            animation.SetInteger("anim", 2);
        }
        else
        {
            if (speedX != 0)
                animation.SetInteger("anim", 1);
            else
                animation.SetInteger("anim", 0);
        }

        //~~~Star collision~~~//
        Collider2D star = Physics2D.OverlapCircle(transform.position, 0.5f, 1 << LayerMask.NameToLayer("Coin"));
        if (star != null)

        {
            //~~~Star Destroy~~~//
            Instantiate(Resources.Load("coinEffect") as GameObject, new Vector3(star.transform.position.x, star.transform.position.y, -1f), Quaternion.identity);
            game_Manager.AddCoin();
            Destroy(star.gameObject);
        }

        //~~~~~~~~~Quaternion~~~~~~~~// 
        if (speedX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (speedX > 0)
            transform.localScale = new Vector3(1, 1, 1);

        //~~~Walking Effects~~~//
        if (!groundCheck)
        {
            sfx_check = true;
        }

        if (groundCheck && sfx_check)
        {
            sfx_check = false;
            game_Manager.PlayJumpSound();
            Instantiate(Resources.Load("Effects/dustStone"), new Vector3(transform.GetChild(0).position.x, transform.GetChild(0).position.y, -10f), Quaternion.identity);
        }

        //~~~~~~~~~Jump~~~~~~~~//
        if (Input.GetKeyDown(KeyCode.Space) && groundCheck)
        {
            JumpMethod(jump_Impulse);
        }

        game_Manager.Add_Score(transform.position.x/5f); // вызываем метод вызова дистанции
    }

    public void JumpMobile()
    {
        bool groundCheck = Physics2D.OverlapBox(transform.GetChild(0).position, child_box_size, 0f, 1 << LayerMask.NameToLayer("ground"));
        if (groundCheck) JumpMethod(jump_Impulse);
    }

    public void attackCheck()
    {
        if (!is_attack_check)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            is_attack_check = true; // булеан на проверку, чтобы урон не багался
            animation.speed = animation_speed; // изменяем анимацию атаки, зависит от скорости атаки

            game_Manager.PlayAttackSound() ; // звук меча
            isAttacking = true;  // даем разрешение на атаку
            attack_set = 4; //  рандом атаки между 1-3;
            attack_time = attack_time_base / animation.speed; // так как анимация равно 0.5, увеличиваем до 1 чтобы небыло повторной анимации
        }
    }

    private void getEffects()
    {
        game_Manager.CameraShake(0.09f, 0.035f, 1f); // Трясение камерой
        Instantiate(sfx_attack, transform.GetChild(1).position, Quaternion.identity); // эффект удара
    }

    void Update()
    {
        if (!allPause && !pause && !isDeath)
        {

            if (Input.GetKeyDown(KeyCode.R) )
            {
                attackCheck();
            }
            //~~~~~~~~~~Attack~~~~~~~~~~// 

            if (isAttacking)
                // функция атаки
                if (attack_time <= 0f)
                {
                    // получает урон
                    isAttacking = false;
                    animation.speed = 1f; // возращаем базовую анимацию атаки

                    //~~~~~~~~~~Attacking~~~~~~~~~~// 

                    Collider2D[] enemy = Physics2D.OverlapCircleAll(transform.GetChild(1).position, 0.6f, 1 << LayerMask.NameToLayer("Monsters"));

                    for (int i = 0; i < enemy.Length; i++)
                    {
                        getEffects();
                        enemy[i].GetComponent<EnemyController>().takeDamage(base_Damage);
                        break;
                    }
                    is_attack_check = false;
                }
                else
                {
                    // наносит урон / анимация
                    attack_time = attack_time - Time.deltaTime;
                    animation.SetInteger("anim", attack_set);
                }
            else
                Moving();
        }
    }

    private IEnumerator fade_time()
    {
        while (true) { 
            //===========================//
            //       healthBar           //
            //===========================//
            if (fadeTime <= 0f)
            {
                healthBar.color = new Color32(255, 0, 0, 0);
                healthBar2.color = new Color32(0, 0, 0, 0);
                is_faded = false;
                break;
            }
            else
            {
                fadeTime = fadeTime - 3f;
                healthBar.color = new Color32(255, 0, 0, (byte)fadeTime);
                healthBar2.color = new Color32(0, 0, 0, (byte)fadeTime);
            }
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    //===============================//
    //         Замораживание         //
    //===============================//
    public void FreezePlayer(bool action)
    {
        pause = action;
        rigidbody.velocity = Vector2.zero;
        rigidbody.isKinematic = action;
        capsule_collider.isTrigger = action;
    }
    //===============================//
    //           Поражение           //
    //===============================//
    public void setLose()
    {
        pause = true;
        rigidbody.velocity = Vector2.zero;

        animation.SetInteger("anim", 3); // death anim
        game_Manager.End_Game();
    }
    //===============================//
    //           Условия поражения              //
    //===============================//
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "spikes")
        {
            game_Manager.PlaySpike_Sound();
            setLose();
        }
        else if(collision.gameObject.tag == "deathzone")
        {
            FreezePlayer(true);
            setLose();
        }
    }
    //===============================//
    //   Установка анимаций атаки    //
    //===============================//
    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = animation.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "attack":
                    attack_time_base = clip.length;
                    break;
            }
        }
    }

}
