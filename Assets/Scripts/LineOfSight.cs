using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public GameObject eyes;

    public List<Transform> visibleTargets = new List<Transform>();



    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void Update()
    {
    }

    // Find victims within the shooter's visible range.
    public void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(eyes.transform.position, viewRadius, targetMask);

        if (targetsInViewRadius.Length == 0)
        {
            return;
        }

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            if (target.tag != "Victim")
                continue;

            Vector3 colCenter = target.TransformPoint(target.GetComponent<CapsuleCollider>().center);
            Vector3 dirToTarget = (colCenter - eyes.transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle/2)
            {
                float distToTarget = Vector3.Distance(eyes.transform.position, colCenter);

                //Debug.DrawRay(eyes.transform.position, dirToTarget*distToTarget, Color.red, 1.0f);

                if (!Physics.Raycast(eyes.transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
                else
                {
                    ;
                    //Debug.Log("Cant hit " + target.name);
                    //RaycastHit hit;
                    //if (Physics.Raycast(eyes.transform.position, dirToTarget, out hit, distToTarget, obstacleMask))
                       //     Debug.Log(hit.transform.name);

                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
