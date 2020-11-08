using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHandAnimation : MonoBehaviour
{
    Animator animator;
    private Animation animation;
    private int raisedHash = Animator.StringToHash("raised");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animation = gameObject.GetComponent<Animation>();
        Debug.Log("TEST");
    }

    // Update is called once per frame
    void Update()
    {
        //         // leave spin or jump to complete before changing
        if (animation.isPlaying)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            if (animator.GetBool(raisedHash)) {
                Debug.Log("LOWER");
                animator.SetBool(raisedHash, false);
            } else {
                Debug.Log("UPPER");
                animator.SetBool(raisedHash, true);
            }
        }
    }
}
