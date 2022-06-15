using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceEnabler : MonoBehaviour
{
    Mace mace_script;
    void Start()
    {
        mace_script = transform.GetChild(0).GetComponent<Mace>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            mace_script.enable_mace(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            mace_script.enable_mace(false);
        }
    }


}
