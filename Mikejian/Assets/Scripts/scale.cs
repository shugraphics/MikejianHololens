using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scale : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vector3 length = this.GetComponent<MeshFilter>().mesh.bounds.size;
        Debug.Log(length);
    }
	
	// Update is called once per frame

}
