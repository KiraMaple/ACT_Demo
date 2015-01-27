using UnityEngine;
using System.Collections;

public class KAAttackBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("this.name:" + this.name + ", other.name:" + other.name);
		if (other.tag == null)
		{
			;
		}
	}

}
