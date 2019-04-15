using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {

	public float duracaoExplosao;

	void Start () {

		Destroy (this.gameObject, duracaoExplosao);

	}

}
