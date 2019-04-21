using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOffset : MonoBehaviour {

    private Renderer    mRenderer;
    private Material    currentMaterial;

    public float        incrementOffset; 
    public float        speed;

    public string       sortingLayer;
    public int          orderInLayer;

    private float       offSet;
   

    // Use this for initialization
    void Start () {
        
        mRenderer = GetComponent<MeshRenderer>();

        mRenderer.sortingLayerName = sortingLayer; // vai ler o nome da layer e rescrever
        mRenderer.sortingOrder = orderInLayer; // vai ler a ordem e rescrever

        currentMaterial = mRenderer.material; // material atual vai rescrever o material antigo
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		
        offSet += incrementOffset;

        currentMaterial.SetTextureOffset("_MainTex", new Vector2(offSet * speed, 0));



	}
}
