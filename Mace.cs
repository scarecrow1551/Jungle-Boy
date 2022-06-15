using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : MonoBehaviour
{

    public float speed;

    public float acceleration;

    public AudioClip trigger_sound;

    public AudioClip slam_sound;

    public Transform[] points;

    private Vector2 box_size;

    private int index;

    private float t = 0f;

    private bool enable;
    void Start()
    {
        box_size = GetComponent<BoxCollider2D>().size;
    }

    public void enable_mace(bool enabler)
    {
        enable = enabler;
    }


    void Update()
    {
        float distance = Vector2.Distance(transform.position, points[index].position);
        if (enable || index == 1)
        {
            if (distance <= 0f)
            {
                t = 0f;
                index = index + 1;
                if (index >= points.Length)
                {
                    index = 0;

                    Sound.PlaySoundGlobal(slam_sound);

                }
                else
                {
                    Sound.PlaySoundGlobal(trigger_sound);

                    enable = false;
                }

            }
        }

        if(index == 1)
            t = t + (speed * (acceleration * 0.00001f));
        transform.position = Vector2.MoveTowards(transform.position, points[index].position, t + speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

        }
    }

}
