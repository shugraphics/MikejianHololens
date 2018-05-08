using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCtr : MonoBehaviour {

    bool flag1;
    bool flag2;
    bool flag3;
    bool flag4;
    bool flag5;
    Animation animation;

    float time;

	// Use this for initialization
	void Start () {
        flag1 = false;
        flag2 = false;
        flag3 = false;
        flag4 = false;
        flag5 = false;
        animation = transform.GetComponent<Animation>();
        time = -1;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (animation.isPlaying == false && flag2 == false)
        {
           animation.Play("成排水管");
            flag2 = true;
        }

        if (animation.isPlaying == false && flag1 == false)
        { 
            animation.Play("成排水管");
            animation["成排水管"].speed = 0;
            flag1 = true;
            time = Time.time;
        }

            if(time > 0 && Time.time - time > 3)
            {
                 animation.Play("HDPE重力排水管");
            }
	}
}
