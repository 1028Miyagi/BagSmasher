using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject target;
    private Vector3 distance;
    public float x, y, z;
    [Header("初期　Y　高さ")]
    public float y2;
    public int[] Movepos;
    

    // Start is called before the first frame update
    void Start()
    {
        distance = new Vector3(x, y2, z);
        transform.position = target.transform.position + distance;
        y = y2;
    }

    // Update is called once per frame
    void Update()
    {
       // y = target.transform.position.y;
        distance = new Vector3(x, y, z);
        transform.position = target.transform.position + distance;

        Transform myTransform = this.transform;

        // 座標を取得
        Vector3 pos = myTransform.position;
        //pos.y = y;
        
        myTransform.position = pos;  // 座標を設定

        for (int i = 0; i < Movepos.Length; i++)
        {
            //C_Move(Movepos[i]);
            
        }
    }

    void C_Move(float pos)
    {
        float a = target.transform.position.x;
        if ( a< pos)
        {
            if (y > y2)
            {
                y -= 0.1f;
            }
        }
        if (a > pos)
        {
            if (y < y2 + 3)
            {
                y += 0.1f;
            }
        }
    }
}
