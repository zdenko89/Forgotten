using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapScript : MonoBehaviour {

    Animator anim;
    public float triggerTime;
    public float activeTime;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("Attack", false);

    }

    void onTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(triggerTime);
        anim.SetBool("Attack", true);
        gameObject.tag = "deadly";
        yield return new WaitForSeconds(activeTime);
        gameObject.tag = "neutralized";
    }
}
