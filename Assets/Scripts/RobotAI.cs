using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RogoDigital.Lipsync;
using System.Diagnostics;

public class RobotAI : MonoBehaviour
{
    public bool isAndi;

    private List<Transform> waypoints = new List<Transform>();
    bool waypointsComplete;
    private int waypointIdx;

    bool missionComplete;

    private NavMeshAgent agent;

    private float movementSpeed;

    private Transform playerTransform;
    private float playerDistance;
    public float waitForPlayerDistance;
    public float startPlayerFollowDistance;
    public float playerWaypointTolerance;
    public LipSync lipSyncCharacter;

    private bool waypointBehaviourComplete;
    private bool performingWaypointBehaviour;

    private bool isPlayerMovingAway;
    private bool disablePlayerFollow;

    private AudioSource audioSource;
    public List<AudioClip> followAudio;
    public float repeatAudioTime;

    private float pauseFollowTill;

    public bool debugSkipAudio = false;


    Animator anim;
    public Animator emergyAnim;
    private Animator Andimator;

    //private LineRenderer lr;

    private void Awake()
    {
        /*lr = gameObject.AddComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;*/

        audioSource = gameObject.GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        missionComplete = false;
        disablePlayerFollow = false;
        performingWaypointBehaviour = false;

        if (isAndi)
            lipSyncCharacter = gameObject.GetComponent<LipSync>();

    }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        playerTransform = PlayerSingleton.Instance.transform;

        pauseFollowTill = Time.time;

        StartCoroutine(SetGoal());
    }

    // Update is called once per frame
    private void Update()
    {

        //debug path planning by showing the path in editor
        /*if (agent.hasPath)
         {
             lr.positionCount = agent.path.corners.Length;
             lr.SetPositions(agent.path.corners);
             lr.enabled = true;
         }
         else
         {
             lr.enabled = false;
         }
 */
        //check if player is running away to switch to following behaviour
        if (Time.time > pauseFollowTill && !disablePlayerFollow)
            IsPlayerMovingAway();
        else
            isPlayerMovingAway = false;
    }

    void IsPlayerMovingAway()
    {
        playerDistance = Vector3.Distance(transform.position, playerTransform.transform.position);

        if (playerDistance > waitForPlayerDistance)
        {
            // if robot performing waypoint action then don't let player move away
            if (performingWaypointBehaviour)
            {
                isPlayerMovingAway = true;
                return;
            }

            // if player moves towards the next waypoint ahead of robot they are not running away
            var rW = Vector3.Distance(transform.position, waypoints[waypointIdx].position);
            var pW = Vector3.Distance(playerTransform.transform.position, waypoints[waypointIdx].position);

            if (pW > rW)
            {
                //Debug.Log("Moving away");
                isPlayerMovingAway = true;
            }
        }
        else
            isPlayerMovingAway = false;
    }

    public bool HasCompletedWaypoints()
    {
        return waypointsComplete;
    }

    public void SetWaypoints(List<Transform> wp)
    {
        waypoints = wp;
        waypointsComplete = false;

    }

    public void ClearWaypoints()
    {
        waypoints.Clear();
        waypointsComplete = false;
    }

    public int GetCurrentWaypointIdx()
    {
        return waypointIdx;
    }

    private IEnumerator SetGoal()
    {
        while (!missionComplete)
        {
            if (waypoints.Count == 0)
            {
                //Debug.Log("Robot waiting for instructions.");
                new WaitForSeconds(1);
            }
            else if (!waypointsComplete)
            {
                //Debug.Log("Robot got waypoints to follow.");
                yield return FollowWaypoints(waypoints);

            }

            yield return null;

        }

        yield break;
    }


    private IEnumerator FollowPlayer()
    {
        int audioClipIdx = 0;
        float prevAudioTime = Time.time;
        Vector3 prevDest = agent.destination;

        // face the player
        LookAt(playerTransform.position);

        while (playerDistance > waitForPlayerDistance)
        {
            // start following if player keeps moving away
            if (playerDistance > startPlayerFollowDistance)
            {

                if (agent.isStopped)
                {
                    //Debug.Log("Following player...");
                    yield return LookAt(playerTransform.position);
                    agent.isStopped = false;
                }

                if (!agent.pathPending)
                {
                    var dir = (playerTransform.position - transform.position).normalized;
                    var dist = Vector3.Distance(playerTransform.position, transform.position);
                    var v = dir * (dist - 1);
                    //agent.destination = transform.position + v;
                }
            }
            else
            {
                if (!agent.isStopped)
                {
                    UnityEngine.Debug.Log("Waiting for player...");
                    agent.destination = transform.position;
                    agent.isStopped = true;
                }

                if (Time.time - prevAudioTime > repeatAudioTime)
                {
                    audioSource.clip = followAudio[audioClipIdx];
                    if (isAndi)
                        lipSyncCharacter.Stop(true);
                    //audioSource.Play();
                    prevAudioTime = Time.time;
                    audioClipIdx = audioClipIdx == 0 ? 1 : 0;
                }

            }

            yield return 0;
        }

        UnityEngine.Debug.Log("Turn to go back");
        yield return LookAt(prevDest);
        UnityEngine.Debug.Log("Going back");
        agent.SetDestination(prevDest);

        yield break;
    }

    private IEnumerator FollowWaypoints(List<Transform> waypoints)
    {
        waypointIdx = 0;
        if (!waypointsComplete)
        {
            //waypointIdx = waypoints.Count - 1;
            agent.SetDestination(waypoints[waypointIdx].position);
        }

        bool startImmediately = false;
        var waypointAction = waypoints[waypointIdx].gameObject.GetComponent<RobotWaypointAction>();
        if (waypointAction != null && waypointAction.GetWaypointActionList().Count > 0 && waypointAction.GetWaypointActionList()[0].type == RobotWaypointAction.Action.Type.StartImmediately)
        {
            startImmediately = true;

        }

        while (waypointIdx < waypoints.Count)
        {
            // start moving again
            if (agent.isStopped)
            {
                //Debug.Log("Continuing tour");
                agent.isStopped = false;
            }

            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
            {
                if (playerDistance < playerWaypointTolerance || startImmediately)
                {
                    waypointBehaviourComplete = false;
                    performingWaypointBehaviour = true;
                    yield return WaypointBehaviour(waypoints[waypointIdx]);
                    performingWaypointBehaviour = false;

                    if (waypointBehaviourComplete)
                    {
                        waypointIdx++;
                        if (waypointIdx == waypoints.Count)
                            break;
                        UnityEngine.Debug.Log("Going to waypoint " + waypointIdx);

                    }
                    else
                    {
                        UnityEngine.Debug.Log("Did not complete waypoint behaviour");
                    }
                    //Debug.Log("B");
                    agent.SetDestination(waypoints[waypointIdx].position);
                    //Debug.Log(agent.nextPosition+" "+agent.destination);

                    waypointAction = waypoints[waypointIdx].gameObject.GetComponent<RobotWaypointAction>();
                    if (waypointAction != null && waypointAction.GetWaypointActionList().Count > 0 && waypointAction.GetWaypointActionList()[0].type == RobotWaypointAction.Action.Type.StartImmediately)
                        startImmediately = true;
                    else
                        startImmediately = false;

                }
            }


            // if player is a runner
            if (isPlayerMovingAway)
            {
                UnityEngine.Debug.Log("Start following playuer");
                yield return FollowPlayer();
                UnityEngine.Debug.Log("Done following playuer");
            }

            yield return null;
        }

        UnityEngine.Debug.Log("Completed waypoints");
        waypointsComplete = true;

        yield break;

    }


    AudioClip TrimAudioClip(AudioClip clip, float start, float stop)
    {
        /* Create a new audio clip */
        int frequency = clip.frequency;
        float timeLength = stop - start;
        int samplesLength = (int)(frequency * clip.channels * timeLength);
        AudioClip newClip = AudioClip.Create(clip.name + "-sub", samplesLength, clip.channels, frequency, false);
        /* Create a temporary buffer for the samples */
        float[] data = new float[samplesLength];
        /* Get the data from the original clip */
        clip.GetData(data, (int)(frequency * start));
        /* Transfer the data to the new clip */
        newClip.SetData(data, 0);
        /* Return the sub clip */

        return newClip;
    }


    private IEnumerator WaypointBehaviour(Transform wp)
    {
        float t;
        var script = wp.gameObject.GetComponent<RobotWaypointAction>();

        if (script == null)
        {
            waypointBehaviourComplete = true;
            yield break;
        }
        var actions = script.GetWaypointActionList();

        foreach (var action in actions)
        {
            switch (action.type)
            {
                case RobotWaypointAction.Action.Type.Serious:
                    if (isAndi)
                    {
                        SkinnedMeshRenderer bod = GetComponentInChildren<SkinnedMeshRenderer>();
                        bod.SetBlendShapeWeight(1, action.param1);
                    }
                    break;
                case RobotWaypointAction.Action.Type.PlayLips:
                    if (isAndi)
                    {
                        LipSyncData lips = action.lipsync1;
                        lipSyncCharacter.Play(lips);
                        //float delayTime2;
                        //delayTime2 = lips.clip.length;

                        //t = Time.time;
                        //while (Time.time - t < delayTime2)
                        //{
                        //    // if player is a runner
                        //    if (isPlayerMovingAway)
                        //        yield break;

                        //    yield return new WaitForSeconds(0.1f);
                        //}
                    }
                    break;
                case RobotWaypointAction.Action.Type.PlayAudio:

                    //Load the correct audio file
                    var audioPath = action.audioPath;
                    AudioClip audioClip;
                    LipSyncData lipSyncData;
                    float delayTime;

                    if (isAndi)
                    {
                        audioPath = "Robot Audio/Andi/" + audioPath;
                        // try lipsync file
                        UnityEngine.Debug.Log("Playing file: " + audioPath + "L");
                        lipSyncData = (LipSyncData)Resources.Load(audioPath + "L");
                        if (lipSyncData)
                        {
                            if (action.param2 - action.param1 > 0)
                            {
                                // does not implement stopping lipsync at certain point before end yet but can start at a random time
                                lipSyncCharacter.PlayFromTime(lipSyncData, action.param1);
                            }
                            else
                                lipSyncCharacter.Play(lipSyncData);

                            if (action.param3 == -1)
                            {
                                delayTime = lipSyncData.clip.length;
                            }
                            else
                                delayTime = action.param3;

                            t = Time.time;
                            while (Time.time - t < delayTime)
                            {
                                // if player is a runner
                                if (isPlayerMovingAway)
                                    yield break;

                                yield return new WaitForSeconds(0.1f);
                            }

                            break;
                        }
                    }
                    else
                    {
                        //6-23-2022 This was changed from Emergy to Andi because they're the same
                        //voice right now.
                        audioPath = "Robot Audio/Andi/" + audioPath;
                    }

                    audioClip = (AudioClip)Resources.Load(audioPath);

                    if (action.param3 == -1)
                        delayTime = audioClip.length;
                    else
                        delayTime = action.param3;

                    if (action.param2 - action.param1 > 0)
                        audioSource.clip = TrimAudioClip(audioClip, action.param1, action.param2);
                    else
                        audioSource.clip = audioClip;

                    if (debugSkipAudio)
                        break;

                    audioSource.Play();

                    t = Time.time;
                    while (Time.time - t < delayTime)
                    {
                        // if player is a runner
                        if (isPlayerMovingAway)
                            yield break;

                        yield return new WaitForSeconds(0.1f);
                    }

                    break;

                case RobotWaypointAction.Action.Type.LookAt:
                    yield return LookAt(action.transform.position);
                    break;

                case RobotWaypointAction.Action.Type.LookAtPlayer:
                    yield return LookAt(PlayerSingleton.Instance.transform.position);

                    break;
                case RobotWaypointAction.Action.Type.Pause:
                    print("Waiting");
                    t = Time.time;
                    while (Time.time - t < action.param1)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }
                    break;

                case RobotWaypointAction.Action.Type.PlayerWaypointTolerance:
                    playerWaypointTolerance = action.param1;
                    break;

                case RobotWaypointAction.Action.Type.WaitForTablet:

                    var tabS = action.gameObj.GetComponentInChildren<TabletUIBehaviour>();
                    while (!tabS.IsSubmitted())
                    {
                        // if player is a runner
                        if (isPlayerMovingAway)
                        {
                            yield break;
                        }
                        yield return new WaitForSeconds(0.25f);
                    }
                    UnityEngine.Debug.Log("Done with tablet");
                    break;
                case RobotWaypointAction.Action.Type.WaitForTrigger:
                    var trigger = action.trigger.GetComponent<RobotWaypointTrigger>();
                    UnityEngine.Debug.Log("Waiting");
                    while (!trigger.IsTriggered())
                    {
                        yield return new WaitForSeconds(0.25f);
                    }
                    UnityEngine.Debug.Log("Moving on");
                    break;

                case RobotWaypointAction.Action.Type.ResetTrigger:
                    action.trigger.GetComponent<RobotWaypointTrigger>().ResetTrigger();
                    break;



                case RobotWaypointAction.Action.Type.SwapRepeatedPhrases:

                    //Load the correct audio file
                    audioPath = action.audioPath;
                    if (isAndi)
                        audioPath = "Robot Audio/Andi/" + audioPath;
                    else
                        audioPath = "Robot Audio/Emergy/" + audioPath;
                    followAudio[0] = (AudioClip)Resources.Load(audioPath);

                    //Load the correct audio file
                    audioPath = action.audioPath2;
                    if (isAndi)
                        audioPath = "Robot Audio/Andi/" + audioPath;
                    else
                        audioPath = "Robot Audio/Emergy/" + audioPath;
                    followAudio[1] = (AudioClip)Resources.Load(audioPath);

                    break;


                case RobotWaypointAction.Action.Type.FollowPlayerUntilTrigger:
                    var trigger2 = action.trigger.GetComponent<RobotWaypointTrigger>();
                    UnityEngine.Debug.Log("Following until trigger");
                    disablePlayerFollow = true;
                    var prevStoppingDistance = agent.stoppingDistance;
                    agent.stoppingDistance = 1.0f;
                    yield return LookAt(playerTransform.position);
                    while (!trigger2.IsTriggered())
                    {
                        UnityEngine.Debug.Log(Vector3.Distance(agent.destination, playerTransform.position));
                        if (Vector3.Distance(agent.destination, playerTransform.position) > waitForPlayerDistance)
                        {
                            agent.destination = transform.position;
                            yield return LookAt(playerTransform.position);
                            agent.destination = playerTransform.position;
                        }
                        yield return new WaitForSeconds(0.25f);
                    }
                    disablePlayerFollow = false;
                    agent.stoppingDistance = prevStoppingDistance;
                    UnityEngine.Debug.Log("Moving on");
                    break;

                case RobotWaypointAction.Action.Type.Animation:
                    if (!isAndi)
                        emergyAnim.SetTrigger(action.animation);
                    if (isAndi)
                    {
                        Andimator = gameObject.GetComponent<Animator>();
                        //Andimator.Play(action.animation, 0, 0.25f);
                        Andimator.SetTrigger(action.animation);
                    }
                    break;
                case RobotWaypointAction.Action.Type.Face:
                    //if (!isAndi)
                    //emergyAnim.SetTrigger(action.animation);
                    if (isAndi)
                    {
                        UnityEngine.Debug.Log("called the face function");
                        Andimator = gameObject.GetComponent<Animator>();
                        //Andimator.Play(action.animation, 0, 0.25f);
                        Andimator.SetBool(action.face, action.faceon);
                        UnityEngine.Debug.Log("set " + action.face + " to " + action.faceon.ToString());
                    }
                    break;

                case RobotWaypointAction.Action.Type.PointAt:
                    break;

                case RobotWaypointAction.Action.Type.StartImmediately:
                    break;


                default:
                    print("Unknown waypoint action.");
                    break;

            }
        }

        waypointBehaviourComplete = true;
        //Debug.Log("Blah...blah...blah...");
        //yield return new WaitForSeconds(1);\

        yield break;
    }


    private IEnumerator WaitTimer(float timer)
    {
        yield return new WaitForSeconds(timer);

    }

    private IEnumerator LookAt(Vector3 loc)
    {
        agent.updateRotation = false;
        Vector3 dir = (loc - transform.position).normalized;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
        //Debug.DrawLine(transform.position, transform.position + dir * 3.0f, Color.green);
        while (Vector3.Angle(transform.forward, dir) > 1)
        {
            //Debug.Log("Turning "+ Vector3.Angle(transform.forward, dir));
            //Debug.DrawLine(transform.position,transform.position + transform.forward * 3.0f, Color.red);
            //Debug.DrawLine(transform.position, transform.position + dir * 3.0f, Color.green);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, agent.angularSpeed * Time.deltaTime);

            if (isAndi)
                anim.SetFloat("Turn", Vector3.Magnitude(dir - (transform.rotation * Vector3.forward)));

            yield return 0;
        }
        agent.updateRotation = true;
        yield break;
    }

    void OnAnimatorMove()
    {
        if (!isAndi)
        {
            return;
        }

        // get position change and smoothen it
        Vector3 dp = agent.nextPosition - transform.position;

        // get smoothed rotation
        float angle = Vector3.SignedAngle(agent.velocity, transform.forward, Vector3.up);

        // Update animations
        if (Time.deltaTime > 1e-5f)
        {
            anim.SetFloat("Forward", agent.velocity.magnitude / (agent.speed * 2.5f)); //factor of 2 to prevent running cause robot cant run
            anim.SetFloat("Turn", angle / 180.0f);

            //Debug.Log(angle/180.0f+ " " + agent.velocity.magnitude / agent.speed);
        }


        if (agent.velocity != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(agent.velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed);
            transform.position = agent.nextPosition;

            // account for animation drifting from navmesh
            if (dp.magnitude > agent.radius)
                agent.nextPosition = transform.position + 0.9f * dp;
        }

    }

}




