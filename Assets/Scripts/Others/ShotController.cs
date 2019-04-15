using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour {

	void OnBecameInvisible(){
		Destroy (this.gameObject);
	}
}
