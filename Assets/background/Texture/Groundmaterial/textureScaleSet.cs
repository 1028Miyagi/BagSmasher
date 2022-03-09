using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureScaleSet : MonoBehaviour
{
    MeshRenderer mesh;
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        Material def = mesh.material;

        mat = new Material(Shader.Find("Legacy Shaders/Diffuse"));
        mat.mainTexture = def.mainTexture;

        mat.mainTextureScale = new Vector2(this.gameObject.transform.localScale.x, -this.gameObject.transform.localScale.y);
        mesh.material = mat;


        
        //defMat.mainTextureScale = new Vector2(this.gameObject.transform.localScale.x, -this.gameObject.transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
