using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public List<GameObject> firePaths;
    public GameObject firePrefab;
    public GameObject smokePrefab;

    public MeshCollider boundingMesh;

    public float spreadDelay;

    public GameObject debugCube;

    public int smokeGridSize;
    public int fireGridSize;

    private List<int> firePointsIdx;
    private List<List<Vector3>> firePoints;
    private Dictionary<Vector3,ParticleSystem> smokePoints;

    private Transform smokePrefabParent;
    private Transform firePrefabParent;

    // Start is called before the first frame update
    void Start()
    {
        // location where the runtime prefabs will be stored to prevent clutter
        smokePrefabParent = transform.Find("Smoke Prefabs");
        firePrefabParent = transform.Find("Fire Prefabs");

        // find points at which to place fire particle system
        firePoints = new List<List<Vector3>>();
        firePointsIdx = new List<int>();
        for (int h = 0; h < firePaths.Count; h++)
        {
            // add index for each path
            firePointsIdx.Add(0);

            // get position of all waypoints in this path
            List<Vector3> positions = new List<Vector3>();
            foreach (Transform child in firePaths[h].transform)
                positions.Add(child.transform.position);

            // interpolate between waypoints to get position of fire
            firePoints.Add(new List<Vector3>());
            for (int i = 0; i < positions.Count - 1; i++)
            {
                float len = Vector3.Distance(positions[i], positions[i + 1]);
                for (float j = 0; j < len; j += fireGridSize)
                {
                    var pos = Vector3.Lerp(positions[i], positions[i + 1], j / len);
                    firePoints[h].Add(pos);
                }

            }
        }

        // find points at which to place smoke particle system
        smokePoints = new Dictionary<Vector3, ParticleSystem>();
        var minVal = boundingMesh.bounds.min;
        var maxVal = boundingMesh.bounds.max;

        for (int i = (int)minVal.x; i < maxVal.x; i += smokeGridSize)
        {
            for (int j = (int)minVal.z; j < maxVal.z; j += smokeGridSize)
            {
                var x = new Vector3(i, 0.0f, j);
                if (boundingMesh.ClosestPoint(x) == x)
                {
                    smokePoints[x] = null;
                    //x.y = 20;
                    //Instantiate(debugCube, x, Quaternion.Euler(-90, 0, 0));
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void startAllFires()
    {
        // Every 2 secs we will emit.
        InvokeRepeating("DoEmit", 2.0f, spreadDelay);
    }


    void DoEmit()
    {
        UpdateSmoke();

        for (int i = 0; i < firePoints.Count; i++)
        {
            // propogate fire
            int idx = firePointsIdx[i];
            if (idx == firePoints[i].Count)
                continue;
            firePointsIdx[i]++;

            GameObject obj = Instantiate(firePrefab, firePoints[i][idx], Quaternion.identity);
            obj.transform.parent = firePrefabParent;
            obj.GetComponent<ParticleSystem>().Play();

            // propogate smoke
            // find the closest smoke grid position
            Dictionary<Vector3, ParticleSystem>.KeyCollection keys = smokePoints.Keys;
            Vector3 smallest = new Vector3(999, 999, 999);
            foreach (var key in keys)
            {
                if (Vector3.Distance(key, firePoints[i][idx]) < Vector3.Distance(smallest, firePoints[i][idx]))
                {
                    smallest = key;
                }
            }
            // if no smoke particle effect at that location
            if (!smokePoints[smallest])
            {
                var objSmoke = Instantiate(smokePrefab, smallest, Quaternion.Euler(-90, 0, 0));
                objSmoke.transform.parent = smokePrefabParent;
                smokePoints[smallest] = objSmoke.GetComponent<ParticleSystem>();
                smokePoints[smallest].Play();
            }
        }
    }

    void UpdateSmoke()
    {
        Vector3[] dirs = { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        Dictionary<Vector3, ParticleSystem>.KeyCollection keys = smokePoints.Keys;
        List<Vector3> updateList = new List<Vector3>();
        foreach (var k in keys)
        {
            if (smokePoints[k] == null)
                continue;

            var em = smokePoints[k].emission;
            if (em.rateOverTime.constant < 10)
            {
                //Debug.Log(em.rateOverTime.constant);
                em.rateOverTime = em.rateOverTime.constant + 1.0f;
            }

            for (int i = 0; i < 4; i++)
            {
                var key = k + dirs[i] * smokeGridSize;
                if (smokePoints.ContainsKey(key))
                {
                    if (smokePoints[key] == null)
                    {
                        updateList.Add(key);
                    }
                    else
                    {
                        em = smokePoints[key].emission;
                        if (em.rateOverTime.constant < 10)
                        {
                            em.rateOverTime = em.rateOverTime.constant + 1.0f;
                        }
                    }
                }
            }
        }
        foreach (var key in updateList)
        {
            var objSmoke = Instantiate(smokePrefab, key, Quaternion.Euler(-90, 0, 0));
            objSmoke.transform.parent = smokePrefabParent;
            smokePoints[key] = objSmoke.GetComponent<ParticleSystem>();
            smokePoints[key].Play();
            //Instantiate(debugCube, key, Quaternion.Euler(-90, 0, 0));
        }
    }
}
