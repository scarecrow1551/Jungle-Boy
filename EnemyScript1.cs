using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript1 : EnemyController
{
    [Header("Настройка эффектов")]
    public GameObject sfx_attack;
    public GameObject sfx_hit;
    public GameObject sfx_blood_explode;

    [Header("Настройка Звук")]
    public AudioClip attack_sound;
    public AudioClip hit_sound;
    public AudioClip death_sound;

    protected override void getAttack()
    {
        float a = transform.position.x;
        float b = player_Script.transform.position.x;
        if (sfx_attack != null)
        {
            GameObject sfx = Instantiate(sfx_attack, attack_point.position, Quaternion.identity);
            if (a > b)
            {
                sfx.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (a < b)
            {
                sfx.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        player_Script.TakeDamage(Damage);
        if (attack_sound != null)
        Sound.PlaySoundGlobal(attack_sound);
    }

    protected override void take_damage_sfx()
    {
        if(sfx_hit != null)
        Instantiate(sfx_hit, transform.position, Quaternion.identity);
        if(hit_sound != null)
        Sound.PlaySoundGlobal(hit_sound);

    }

    protected override void destroy_enemy_sfx()
    {
        if(sfx_blood_explode != null)
        Instantiate(sfx_blood_explode, transform.position, Quaternion.identity);
        if(death_sound != null)
        Sound.PlaySoundGlobal(death_sound);
        for(int i = 0; i < Random.RandomRange(3, 6); i++)
        {
            GameObject coin = Instantiate(Resources.Load("CoinRigidBody"), transform.position, Quaternion.identity) as GameObject;
            coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-300f, 300f), Random.Range(300f, 500f)));
        }
        FindObjectOfType<GameManager>().Monster_Kill();
    }

}
