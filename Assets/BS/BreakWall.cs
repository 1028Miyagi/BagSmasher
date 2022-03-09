using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BreakWall : MonoBehaviour
{
    public int min = -5;
    public int max = 5;

    public Player Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            //destroyObject();
        }
    }
    public void destroyObject()
    {
        var random = new System.Random();
        
        foreach (Transform child in this.transform)
        {
            // ˆê‚Â‚¸‚Â”jŠü‚·‚é
            Destroy(child.gameObject,4f);
        }
        gameObject.GetComponentsInChildren<Rigidbody>().ToList().ForEach(r => {
            r.isKinematic = false;
            r.transform.SetParent(null);
            //r.gameObject.AddComponent<AutoDestroy>().time = 2f;
            var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
            r.AddForce(vect, ForceMode.Impulse);
            r.AddTorque(vect, ForceMode.Impulse);
            //Destroy(r, 3f);
        });
        Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullets"))
        {
            Player.breakWall();
            print("Ball");
            destroyObject();
        }

        if (other.CompareTag("Attacktag"))
        {
            Player.breakWall();
            destroyObject();
        }
    }
}
