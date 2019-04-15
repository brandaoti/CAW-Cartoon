using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private GameController 	_gameController;

	private Rigidbody2D 	playerRB;
	private SpriteRenderer	playerSR; 

	[Header("Config. Plane")]

	public SpriteRenderer	contrailSR; // TODO ir aumentando a força da fumaça no momento da decolagem
	public GameObject 		shadowGO;  // TODO desabilitar a sombra na hora que for decolar
	public Color			invinvibleColor; 

	public float 			speed = 5f; 
	public float 			tilt  = 3f; // Fazer o avião inclinar 

	[Header("Config. Gun | Bullets")]

	public tagBullets	tag_Bullet;
	public Transform	shotSpawn;

	public int 			idBullet;    	     // Identificador da bala
	public float 		shotSpeed = 10f;    // Velocidade do tiro, iniciada com 10
	public float 		fireRate  = 0.3f;  // Taxa de tiro por segundos, "Um deley entre os tiros"
	private float 		nextFire  = 0.0f; // 

	//private bool 		isFire;

	// Use this for initialization
	void Start () {
		
		_gameController = FindObjectOfType (typeof(GameController)) as GameController;

		_gameController._playerController = this;
		_gameController.isAlivePlayer = true;


		playerRB = GetComponent<Rigidbody2D> ();
		playerSR = GetComponent<SpriteRenderer> ();
		
	}
	
	// Update is called once per frame
	void Update () {

		if(_gameController.currentState == gameState.gameplay){
			Move ();

			// Time.time, fornece o tempo que o jogo esta sendo executado em segundos desde o inicio do jogo
			// Vai pegar o Time atual do jogo ao atirar, e vai verificar se é maior que proximo tiro
			if(Input.GetButton ("Fire1") && Time.time > nextFire){ 

				// O proximo tiro vai receber o tempo do ultimo tiro + o deley entre os tiros
			
				nextFire = Time.time + fireRate;

				Shot ();
			}
	    }
	}

	void Move(){

		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		/*Vector2 movement = new Vector2 (horizontal, vertical);
		playerRB.velocity = movement * speed;*/

		/*playerRB.position = new Vector2 
		{
			Mathf.Clamp (playerRB.position.x, _gameController.limiteMinX, _gameController.limiteMaxX),
			Mathf.Clamp (playerRB.position.y, _gameController.limiteMinY, _gameController.limiteMaxY)
		};*/

		transform.rotation = Quaternion.Euler (new Vector3 (0.0f, 0.0f, playerRB.velocity.x * -tilt));

		playerRB.velocity = new Vector2 (horizontal * speed, vertical * speed);
	}

  	void Shot(){//TODO - precisa melhorar o codigo

		//isFire = true;
		//StartCoroutine ("ShotDelayTimer");

		GameObject temp = Instantiate (_gameController.bulletPrefab[idBullet]);

		temp.transform.tag = _gameController.aplicarTag (tag_Bullet);

		temp.transform.position = shotSpawn.position;

		temp.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, shotSpeed);
	}

	/*IEnumerator ShotDelayTimer(){ 

		yield return new WaitForSeconds (shooting_Delay_Timer);

		isFire = false;

		//Shot ();

		StartCoroutine ("ShotDelayTimer");
	}*/

	void OnTriggerEnter2D(Collider2D c){

		switch(c.gameObject.tag){
		case "EnemyShot":

			Destroy (c.gameObject);

			_gameController.hitPlayer ();

			break;
		}
	}

	IEnumerator Invinvible(){ //TODO  fazer um colisor e desabilitar e so habilitar quando parar de piscar ao ser atingido

		Collider2D colisor = GetComponent<Collider2D> ();

		colisor.enabled = false;
		playerSR.color = invinvibleColor;

		yield return new WaitForSeconds (_gameController.tempoInvencivel);
		colisor.enabled = true;

		playerSR.color = Color.white;

	}
}
