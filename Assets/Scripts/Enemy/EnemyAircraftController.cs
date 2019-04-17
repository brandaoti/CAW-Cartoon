using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction{

	none, up, down, right, left

}

public class EnemyAircraftController : MonoBehaviour {

    private GameController  _gameController;

    //private Rigidbody2D enemyRB;

	public Direction direction;

	public float enemySpeed;
	public float curveLocation; 	    // Vai ser definido via objeto o ponto do local exato que iniciara a curva
	public float degreesRotation; 	   // Vai ser definido o grau da curva. Exemplo 90°
    public float tilt;
	public float routeChange;  		  // Vai mudar o rota, definindo se vai virar pra direita ou esquerda, valor (-1, 1).

	private float zRotation;		// Vai receber o valor da variavel "routeChange"
	private float countRotation;   // Vai fazer apenas a contagem pra startar a rotação

	private bool isCurve;

    [Header("Config. Arma | Tiro")]

    public tagBullets   tag_Bullet; 
    public Transform    gunPosition; // Posição da arma
    public int          id_Bullet; // Id da bala que o tanque irá assumir
    public float        shotSpeed; // Ao sair da arma
    public float        shotTime; // Temporizador entre disparo

	// Use this for initialization
	void Start () {

        _gameController = FindObjectOfType(typeof(GameController)) as GameController;
		
		zRotation = transform.eulerAngles.z;

	}

	// Update is called once per frame
	void Update () {
		
		ChangeRotation ();
	
	}

    void shotEnemy(){


        if(_gameController.isAlivePlayer == true){
            
            gunPosition.up = _gameController._playerController.transform.position - transform.position;

            GameObject temp = Instantiate(_gameController.bulletPrefab[id_Bullet], gunPosition.position, gunPosition.localRotation);
            temp.transform.tag = _gameController.aplicarTag(tag_Bullet);

            temp.GetComponent<Rigidbody2D>().velocity = gunPosition.up * shotSpeed;
        }
       

    }

	void ChangeRotation (){

		switch(direction){

			// Inimigo ir para baixo
		case Direction.up:

			if(transform.position.y >= curveLocation && isCurve == false){
				isCurve = true;
			}

			if(isCurve == true && countRotation < degreesRotation){

				zRotation += routeChange; // primeiro vai escolher qual rota seguir

				transform.rotation = Quaternion.Euler (new Vector3(0, 0, zRotation));

				// count vai sempre ser positivo

				if(routeChange < 0){ 
					countRotation += (routeChange * -1); // valor sendo negativo, vai ficar positivo
				}else {
					countRotation += routeChange;
				}
			}
			transform.Translate (Vector3.up * enemySpeed * -1 * Time.deltaTime);

		break;

			// Inimigo ir para cima
		case Direction.down:

			if (transform.position.y <= curveLocation && isCurve == false) {
				isCurve = true;
			}

			if (isCurve == true && countRotation < degreesRotation) {

				zRotation += routeChange; 

				transform.rotation = Quaternion.Euler (new Vector3(0, 0, zRotation));
              

				if(routeChange < 0){
					countRotation += (routeChange * -1);
				} else {
					countRotation += routeChange;
				}
			}          
			transform.Translate (Vector3.down * enemySpeed * Time.deltaTime);

			break;
		
            // Inimigo ir para esquerda
		case Direction.left:

			if (transform.position.x <= curveLocation && isCurve == false) {
				isCurve = true;
			}

			if (isCurve == true && countRotation < degreesRotation) {

				zRotation += routeChange;

				transform.rotation = Quaternion.Euler (new Vector3 (0, 0, zRotation));

				if (routeChange < 0) {
					countRotation += (routeChange * -1);
				} else {
					countRotation += routeChange;
				}

			}
			transform.Translate (Vector3.down * enemySpeed * Time.deltaTime);

			break;

            // Inimigo ir para direita 
		case Direction.right:

			if (transform.position.x >= curveLocation && isCurve == false) {
				isCurve = true;
			}

			if (isCurve == true && countRotation < degreesRotation) {

				zRotation += routeChange;

				transform.rotation = Quaternion.Euler (new Vector3 (0, 0, zRotation));

				if (routeChange < 0) {
					countRotation += (routeChange * -1);
				} else {
					countRotation += routeChange;
				}
			}
			transform.Translate (-Vector3.up * enemySpeed * Time.deltaTime);

			break; 
		}
	}

    void OnTriggerEnter2D(Collider2D c){

        switch(c.gameObject.tag){
        case "PlayerShot":

            Destroy(c.gameObject);

            GameObject temp = Instantiate(_gameController.explosaoPrefab, transform.position, transform.localRotation);

            temp.transform.parent = _gameController.cenario;

            Destroy(this.gameObject);

        break;
        }
    }

    IEnumerator ShootingTimer(){

        yield return new WaitForSeconds(shotTime);

        shotEnemy();

        StartCoroutine("ShootingTimer");
    }

    void OnBecameVisible(){
        StartCoroutine("ShootingTimer");
    }

	void OnBecameInvisible(){
		Destroy (this.gameObject);
	}
}































