using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Header("Базовые Настройки")]
    public float Damage;
    public float Speed;
    public float Health;
    public float Attack_Range;
    public float Attack_Cooldown;
    public float Agro_Distance;

    [Header("Прочие Настройки")]
    public float pos_y;
    public bool is_RightFlip;

    // Приватные переменные

    //         Атака
    protected float attack_cd;
    protected float attack_duration_base;
    protected float attack_duration;
    protected bool  is_attack;

    //         Жизнь
    protected float health;

    //         прочее

    private float[] Points_X = new float[2];
    protected bool is_move;


    // приватные компоненты
    protected Rigidbody2D rb;
    protected Animator anim;
    protected PlayerScript player_Script;
    protected Transform attack_point;
    protected Vector3 localScale;
    // приватные компоненты
    private Image[] UI_health = new Image[2];
    private float color_max;
    private bool colored;


    protected enum Stage { IDLE, WALK, ATTACK, DEATH };
    protected Stage stage = Stage.WALK;

    void Start()
    {
        rb            = GetComponent<Rigidbody2D>();
        anim          = GetComponent<Animator>();
        player_Script = FindObjectOfType<PlayerScript>();
        attack_point  = transform.GetChild(0);
        localScale    = transform.localScale;
        // настройка шкалу здоровья
        UI_health[0] = transform.GetChild(1).transform.GetChild(0).GetComponent<Image>(); // black background
        UI_health[1] = transform.GetChild(1).transform.GetChild(1).GetComponent<Image>(); // fill Image

        UI_health[0].color = new Color(0, 0, 0, 0f);
        UI_health[1].color = new Color(1f, 0, 0, 0f);

        // настройка переменных базовых

        Points_X[0] = transform.GetChild(2).position.x;
        Points_X[1] = transform.GetChild(3).position.x;
        health = Health;
        

        UpdateAnimClipTimes();
    }

    private void Move()
    {
        float x = transform.position.x;
        float y = transform.position.y;

        Vector3 pos = transform.position + Vector3.right * Agro_Distance;
        Vector3 playerPos = transform.position;
        pos.y = pos.y-pos_y;
        playerPos.y = playerPos.y-pos_y;
       RaycastHit2D hit = Physics2D.Linecast(playerPos + Vector3.left * Agro_Distance, pos, 1 << LayerMask.NameToLayer("Player"));
        if (hit.collider != null)
            is_move = true;
        else
            is_move = false;
        stage = Stage.WALK;
        Debug.DrawLine(playerPos + Vector3.left * Agro_Distance, pos, Color.blue);


        if (is_move)
        {
            Vector2 posA = transform.position;
            Vector2 posB = player_Script.transform.position;

            if (posA.x > posB.x)
                Speed = Mathf.Abs(Speed) * (-1f);
            else
                Speed = Mathf.Abs(Speed);

            transform.position = new Vector2(transform.position.x + Speed * Time.deltaTime, transform.position.y);
            anim.SetInteger("anim", 1);
            Flip();
        }
        else
        {

            if(transform.position.x > Points_X[1])
            {
                Speed = Mathf.Abs(Speed) * (-1f);
            }
            else if (transform.position.x < Points_X[0])
            {
                Speed = Mathf.Abs(Speed);
            }

            transform.position = new Vector2(transform.position.x + Speed * Time.deltaTime, transform.position.y);
            anim.SetInteger("anim", 1);
            Flip();
        }

    }



    protected virtual void getAttack()  { return; }
    protected virtual void take_damage_sfx() { return;}
    protected virtual void destroy_enemy_sfx() { return; }



    private IEnumerator img_transparent()
    {
        colored = true;
        while (true)
        {
            UI_health[0].color = new Color(0f, 0f, 0f, color_max);
            UI_health[1].color = new Color(255f, 0f, 0f, color_max);
            if (color_max <= 0) break;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            color_max = color_max - Time.fixedDeltaTime;
        }
        colored = false;
    }

    public void takeDamage( float damage)
    {
        take_damage_sfx();
        health = health - damage;
        UI_health[1].fillAmount = health / Health;

        color_max = 1f;

        if (!colored)
            StartCoroutine(img_transparent());
        if (health <= 0f)
        {
            destroy_enemy_sfx();
            Destroy(gameObject);
        }

    }




    private void Attack()
    {
        if (is_attack)
        {
            if (attack_duration <= 0)
            {
                is_attack = false;
                getAttack();
            }
            else
            {
                attack_duration = attack_duration - Time.deltaTime;
                anim.SetInteger("anim", 2); // attack animation
            }
        }
        else
        {
            if (attack_cd <= 0)
            {
                is_attack = true;
                attack_cd = Attack_Cooldown;
                attack_duration = attack_duration_base;
            }
            else
            {
                attack_cd = attack_cd - Time.deltaTime;
                anim.SetInteger("anim", 0); // idle animation
            }
        }
    }

        void Update()
        {
            float x = transform.position.x;
            float y = transform.position.y;

            if (!PlayerScript.allPause)
            {
                float distance = Vector2.Distance(player_Script.transform.position, transform.position);

                if (distance <= Attack_Range && !PlayerScript.isDeath)
                    stage = Stage.ATTACK;
                else if (!PlayerScript.isDeath)
                    stage = Stage.WALK;
                else
                    stage = Stage.IDLE;

                switch (stage)
                {
                    case Stage.IDLE:
                            anim.SetInteger("anim", 0); // idle animation
                        break;

                    case Stage.WALK:
                            Move();
                        break;

                    case Stage.ATTACK:
                        // Flip
                        if (player_Script.transform.position.x > transform.position.x)
                            localScale.x = Mathf.Abs(localScale.x);
                        else
                            localScale.x = Mathf.Abs(localScale.x) * (-1f);
                        transform.localScale = localScale;
                        Attack();
                        break;
                }
            }
        }


    private void Flip()
    {
        if(Speed > 0)
        {
            if (is_RightFlip)
                localScale.x = Mathf.Abs(localScale.x) * (-1f);

            else
                localScale.x = Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
        else if(Speed < 0)
        {
            if (is_RightFlip)
                localScale.x = Mathf.Abs(localScale.x);
            else
                localScale.x = Mathf.Abs(localScale.x) * (-1f);
            transform.localScale = localScale;

        }

    }
    protected void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "attack":
                    attack_duration_base = clip.length;
                    break;
            }
        }
    }
}
