using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum tagBullets{
	inimigo, player
}

public enum gameState{
	intro, gameplay
}



public class GameController : MonoBehaviour {

	public PlayerController	_playerController;

	[Header("Config. Limites mapa")]

	public Transform	limiteMaxX;
	public Transform	limiteMinX;
	public Transform	limiteMaxY;
	public Transform	limiteMinY;
	public Transform	limiteFinal; // Limite maximo que a fase vai chegar
	public Transform	cenario;
	public float 		velocidadeCenario;
		

	[Header("Config. Prefabs")]

	public GameObject[] bulletPrefab;
	public GameObject 	explosaoPrefab;
	public GameObject	playerPrefab;

	[Header("Config. Player")]

	public bool 		isAlivePlayer;
	public Transform 	playerSpawn;
	public int			vidasExtras;
	public float 		tempoSpawn;
	public float 		tempoInvencivel;

	[Header("Config. Intro | GamePlay | Fim Fase")]
	public gameState	currentState;
	public Transform	localPartida;
	public Transform	localDecolagem;

	public float 		tamanhoInicialNave;
	public float 		tamanhoOriginal;

	public float		velocidadeDecolagem;
	private float 		velocidadePartida;
	private bool 		isDecolar;

	public Color		corInicialFumaca;
	public Color		corFinalFumaca;

	// Use this for initialization
	void Start () {

		StartCoroutine ("introFase");

		//_playerController = FindObjectOfType<PlayerController> () as PlayerController;


	}
	
	// Update is called once per frame
	void Update () {
		if(isAlivePlayer == true){
			limitarMovimento ();
		}

		if(isDecolar == true && currentState == gameState.intro){

			movimentarIntro ();

			if(_playerController.transform.position == localDecolagem.position){

				StartCoroutine ("decolar");
				currentState = gameState.gameplay; 
			}
			_playerController.contrailSR.color = Color.Lerp (corInicialFumaca, corFinalFumaca, 0.1f);
		}
	}

	void LateUpdate(){

		if(currentState == gameState.gameplay){
			movimentarCenario ();
		}

	}

	void movimentarIntro(){

		_playerController.transform.position = Vector3.MoveTowards 
			(_playerController.transform.position, localDecolagem.position, velocidadePartida * Time.deltaTime);
	}

	void limitarMovimento(){

		float posX	= _playerController.transform.position.x;
		float posY	= _playerController.transform.position.y;

		// Verificar se o player chegou na posição definida HORIZONTAL
		if(posX > limiteMaxX.position.x){
			_playerController.transform.position = new Vector3 (limiteMaxX.position.x, posY, 0);

		}else if(posX < limiteMinX.position.x){
			_playerController.transform.position = new Vector3 (limiteMinX.position.x, posY, 0);
		}

		// Verificar se o player chegou na posição definida VERTICAL
		if(posY > limiteMaxY.position.y){
			_playerController.transform.position = new Vector3 (posX, limiteMaxY.position.y, 0);

		} else if(posY < limiteMinY.position.y){
			_playerController.transform.position = new Vector3 (posX, limiteMinY.position.y, 0);
		}
	}

	void movimentarCenario(){

		cenario.position = Vector3.MoveTowards (cenario.position, new Vector3 (cenario.position.x, limiteFinal.position.y, 0),
			velocidadeCenario * Time.deltaTime);

	}

	public string aplicarTag(tagBullets tag){

		string t = null;

		switch(tag){
		case tagBullets.player:
			t = "PlayerShot";
			break;

		case tagBullets.inimigo:
			t = "EnemyShot";
			break;
		}
		return t;
	}

	public void hitPlayer(){

		isAlivePlayer = false;

		GameObject temp = Instantiate (explosaoPrefab, _playerController.transform.position, explosaoPrefab.transform.localRotation);

		Destroy (_playerController.gameObject);

		vidasExtras -= 1;

		if(vidasExtras >= 0){
			StartCoroutine ("instanciarPlayer");
		} else {
			print ("Game Over");
		}
	}

	IEnumerator instanciarPlayer(){

		yield return new WaitForSeconds (tempoSpawn);
		GameObject temp = Instantiate (playerPrefab, playerSpawn.position, playerSpawn.localRotation);

		yield return new WaitForEndOfFrame ();
        _playerController.StartCoroutine ("Invincible");

		_playerController.shadowGO.SetActive (true);
	}

	IEnumerator introFase(){

		_playerController.contrailSR.color = corInicialFumaca;
		_playerController.shadowGO.SetActive (false); // desativar sombra

		// A nave vai receber um novo tamanho inicial nas 3 posições do scale
		_playerController.transform.localScale = new Vector3(tamanhoInicialNave, tamanhoInicialNave, tamanhoInicialNave);

		// Definir local de partida da nave
		_playerController.transform.position = localPartida.position;

		// A nave ira decolar apos a conclusao da animação
		yield return new WaitForEndOfFrame ();
		isDecolar = true;
		print ("Decolagem Autorizada!");

		//Definir um contador pra ir aumentando a velocidade atual de partida;
		for(velocidadePartida = 0; velocidadePartida < velocidadeDecolagem; velocidadePartida += 0.1f){

			print (velocidadePartida);
			yield return new WaitForSeconds (0.1f);
		}

	}

	IEnumerator decolar(){

		_playerController.shadowGO.SetActive (true); //Ativar sombra

		tamanhoInicialNave = _playerController.transform.localScale.x;

		for (float scale = tamanhoInicialNave; scale < tamanhoOriginal; scale += 0.025f){

			_playerController.transform.localScale = new Vector3 (scale, scale, scale); //Aumentar Nave
			_playerController.shadowGO.transform.localScale = new Vector3(scale, scale, scale); // Aumentar sombra

			_playerController.contrailSR.color = Color.Lerp (_playerController.contrailSR.color, corFinalFumaca, 0.2f);

			yield return new WaitForSeconds (0.1f);
		}


	}
}
