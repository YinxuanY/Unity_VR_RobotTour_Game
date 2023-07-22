using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RogoDigital.Lipsync;

public class RobotWaypointAction : MonoBehaviour
{

    [System.Serializable]
    public struct Action
    {
        public enum Type
        {
            PlayAudio,
            PointAt,
            LookAt,
            LookAtPlayer,
            Pause,
            WaitForTablet,
            WaitForTrigger,
            ResetTrigger,
            DestroyCollider,
            SwapRepeatedPhrases,
            FollowPlayerUntilTrigger,
            StartImmediately,
            Animation,
            Face,
            Serious,
            PlayerWaypointTolerance,
            PlayLips

        }

        public Type type;
        public string audioPath;
        public string audioPath2;
        public AudioClip audioClip;
        public AudioClip audioClip2;
        public Transform transform;
        public float param1;
        public float param2;
        public float param3;
        public Collider trigger;
        public GameObject gameObj;
        public string animation;
        public string face;
        public bool faceon;
        public LipSyncData lipsync1;
        
    }


    public List<Action> actionList;

    private void Awake()
    {
    }

    private void Update()
    {
     
    }
    public List<Action> GetWaypointActionList()
    {
        return actionList;
    }
}
