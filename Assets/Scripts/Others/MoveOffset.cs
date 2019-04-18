using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOffset : MonoBehaviour {

    private Renderer    mRenderer;
    private Material    currentMaterial;

    public float        incrementOffset; 
    public float        speed;

    private float       offSet;

    // Use this for initialization
    void Start () {

        mRenderer = GetComponent<MeshRenderer>();

        currentMaterial = mRenderer.material;
    }
	
	// Update is called once per frame
	void Update () {
		
        offSet += incrementOffset;

        currentMaterial.SetTextureOffset("_MainTex", new Vector2(offSet * speed, 0));


	}
}
