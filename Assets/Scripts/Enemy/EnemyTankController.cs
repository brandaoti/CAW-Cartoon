using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankController : MonoBehaviour {

	private GameController 		_gameController;
	private PlayerController 	_playerController;

	private Rigidbody2D tankRB;

	[Header("Config. Arma | Tiro")]

	public tagBullets	tag_Bullet; 
	public Transform	gunPosition; // Posição da arma
	public int 			id_Bullet; // Id da bala que o tanque irá assumir
	public float 		shooting_Velocity; // Ao sair da arma
	public float 		shooting_Delay_Timer; // Temporizador entre disparo


	void Start () {

		_gameController = FindObjectOfType(typeof(GameController)) as GameController;


		tankRB = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {

	}

	void atirar(){

		if(_gameController.isAlivePlayer == true){

			gunPosition.up = _gameController._playerController.transform.position - transform.position;

			GameObject temp = Instantiate (_gameController.bulletPrefab[id_Bullet], gunPosition.position, gunPosition.localRotation);
			temp.transform.tag = _gameController.aplicarTag (tag_Bullet);

			//temp.transform.position = gunPosition.position;

			temp.GetComponent<Rigidbody2D> ().velocity = gunPosition.up * shooting_Velocity;
		}


	}

	IEnumerator tmpDisparo(){

		yield return new WaitForSeconds (shooting_Delay_Timer);

		atirar ();

		StartCoroutine ("tmpDisparo");
	}

    void OnTriggerEnter2D(Collider2D c){

        switch(c.gameObject.tag){
        case "PlayerShot":

            Destroy(c.gameObject);

            GameObject temp = Instantiate(_gameController.explosaoPrefab, transform.position, transform.localRotation);

            Destroy(this.gameObject);

        break;
        }
    }

	void OnBecameVisible(){

		StartCoroutine ("tmpDisparo");
	}

	void OnBecameInvisible(){

		Destroy (this.gameObject);
	}
  
}