using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> decals;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InsertRandom(Vector3 pos)
    {
        pos.y += 0.001f; //Hack otherwise it blends with the ground
        int idx = Random.Range(0, decals.Count);
        GameObject clone = Instantiate(decals[idx], pos, Quaternion.identity);
        clone.SetActive(true);
    }
}
