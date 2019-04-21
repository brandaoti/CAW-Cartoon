using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum Dir{
    right
}*/

public class HUDEnemyController : MonoBehaviour {

    public Transform    HUDEnemy;
    public Transform[]  checkPoint;


    public float speed;
    public float delayMoviment;

    private int  IDCheckPoint;
    private bool isMoviment;

    /*[Header("Config. Gun and Shot")]

    public tagBullets   tagBullet;
    public Transform    gunPosition;
    public GameObject   bullet;
    public float        shotSpeed;
    public float        shotTime;
*/


	// Use this for initialization
	void Start () {

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 40;
        
        StartCoroutine("StartMoviment");
	}
	
	// Update is called once per frame
	void Update () {
        Moviment();
	}

    void Moviment(){
        if (isMoviment == true){
            HUDEnemy.localPosition = Vector3.MoveTowards(HUDEnemy.localPosition, checkPoint[IDCheckPoint].position, speed * Time.deltaTime);

            if(HUDEnemy.localPosition == checkPoint[IDCheckPoint].position){
                isMoviment = false;
                StartCoroutine("StartMoviment");
            }
        }
    }
        

    IEnumerator StartMoviment(){

        IDCheckPoint += 1;

        if(IDCheckPoint >= checkPoint.Length){
            IDCheckPoint = 0;
        }

        yield return new WaitForSeconds(delayMoviment);
        isMoviment = true;
    }
        
}
