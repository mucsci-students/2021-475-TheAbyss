using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SharkAI : MonoBehaviour
{
    public float sharkSwimmingSpeed = 3.0f;
    public float sharkStraightlineSight = 20f;
    public float obstacleDetectionDistance = 70f;
    public float obstacleAvoidanceRotationSpeed = 20f;

    public float runAwayDistance = 100f;

    public int patrolNodesCount = 10;
    private bool hasInitializedPatrol = false;
    private int completedPatrols = 0;
    private int patrolPosition = 0;
    private Transform[] patrolNodes;

    public float runAwayMaxTime = 10f;
    private float runAwayTimer;

    public GameObject patrolNode;

    private float interestArriveDistance = 5.0f;

    public GameObject player;
    public float patrolRadius = 5f;

    public bool playerIsInVisionCone = false;
    public bool playerCanBeRaycasted = false;
    private bool hasStartedChasing = false;

    public Transform currentPointOfInterest;
    private StateManager sm;
    private bool somethingIsInTheWay = false;
    private bool terrainTooClose = false;
    private RaycastHit obstacleHit;

    private Transform runToThisPoint;

    private Vector3 turnDirection;
    private Transform lastPointOfInterest;

    private GameObject gameManager;
    private MusicManager musicManager;

    public float sightProcessingSpeed = 1.0f;
    private float brainProcessingTime = 0.0f;

    private AudioSource jumpscareSound;

    private bool isClimax = false;

    public AudioSource sharkPainSFX;

    // Start is called before the first frame update
    void Start()
    {
        sm = GetComponent<StateManager>();
        patrolNodes = new Transform[patrolNodesCount];
        gameManager = GameObject.Find("GameManager");
        musicManager = gameManager.GetComponent<MusicManager>();
        jumpscareSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        DetermineAction();
        UpdateStateLogic();
    }

    public void SetInterestPoint(Transform newInterestPoint)
    {
        sm.hasArrivedAtPoint = false;
        currentPointOfInterest = newInterestPoint;
    }

    public void TeleportSharkToTransform(Transform teleport)
    {
        SetInterestPoint(teleport);
        transform.position = teleport.position;
    }

    private bool SeePlayer()
    {   
        if(playerCanBeRaycasted && playerIsInVisionCone)
        {
            brainProcessingTime += Time.deltaTime;
            if(brainProcessingTime >= sightProcessingSpeed)
            {
                musicManager.SetSharkChasing(true);
                return true;
            }
            musicManager.SetSharkChasing(false);
            hasStartedChasing = false;
            return false;
        }
        else
        {
            brainProcessingTime = 0.0f;
            musicManager.SetSharkChasing(false);
            hasStartedChasing = false;
            return false;
        }
    }

    void UpdateStateLogic()
    {
        if(!isClimax)
        {
            sm.canSeePlayer = SeePlayer();
        }
        else
        {
            sm.canSeePlayer = true;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            sm.canAttackPlayer = true;
        }
    }

    // Determine current state from state manager
    void DetermineAction()
    {
        State c = sm.currentState;
        if(c is PatrolState)
        {
            Patrol();
        }
        else if(c is MoveTowardsPointState)
        {
            MoveTowardsPoint();
        }
        else if(c is ChaseState)
        {
            ChasePlayer();
        }
        else if(c is RunAwayState)
        {
            RunAway();
        }
        else if(c is KillState)
        {
            KillPlayer();
        }
        else
        {
            Debug.Log("Unknown state: " + c);
        }
    }

    private void Patrol()
    {
        // Initialize patrol path if one isn't already created
        if(!hasInitializedPatrol)
        {
            Debug.Log("INITIALIZE! I DIE FOR THE CAUSE");
            InitializePatrol();
        }

        // If nothings in the way, turn and move towards the node
        if(!somethingIsInTheWay)
        {
            Vector3 relativePos = patrolNodes[patrolPosition].position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        }

        transform.Translate(Vector3.forward * Time.deltaTime * sharkSwimmingSpeed);

        // Obstacle avoidance
        RaycastHit r;
        if(Physics.Raycast(transform.position, patrolNodes[patrolPosition].position - transform.position, out r))
        {
            if(!r.collider.gameObject.CompareTag("PathfindingNode"))
            {
                DetectObstaclesAndTerrain();
            }  
        }

        // Terrain and obstacle logic
        NavigateObstaclesAndTerrain(patrolNodes[patrolPosition]);

        // Determine if we've arrived at a given node
        if(Vector3.Distance(transform.position, patrolNodes[patrolPosition].position) <= 5.0f)
        {
            patrolPosition++;
            if(patrolPosition >= patrolNodesCount)
            {
                ++completedPatrols;
                patrolPosition = 0;
            }
        }
    }

    // patrolPath must be formatted this way:
    // -Empty GameObject 
    //      - Node 1
    //      - Node 2
    //      - ...
    //      - Node n
    // Order of children is order that the shark will patrol in
    public void InitializeCustomPatrolPath(GameObject patrolPath)
    {
        Transform[] newPatrol = patrolPath.GetComponentsInChildren<Transform>();
        patrolNodes = newPatrol;
        patrolPosition = 0;
        hasInitializedPatrol = true;
    }

    private void InitializePatrol()
    {
        // Reset patrol specific values
        completedPatrols = 0;
        patrolPosition = 0;

        // Set the center to the point of interest
        Vector3 center = currentPointOfInterest.position;
        
        for(int i = 0; i < patrolNodesCount; ++i)
        {
            // Generates angle in the circle that the ith node needs to be placed at
            int angle = 360 / patrolNodesCount * i;
            // pos is a given location in the imaginary circle with patrolRadius radius and at "angle" angle
            Vector3 pos = CirclePosition(center, patrolRadius, angle);
            
            while(true)
            {
                RaycastHit rh;
                // Determine if the node placement would intersect with another collider
                if(Physics.CheckSphere(pos, 1f))
                {
                    Debug.Log("to be placed is inside something");
                    // Move up until we're out of the object
                    pos += new Vector3(0.0f, 1.0f, 0.0f);
                }
                // If there's nothing in the check sphere do a raycast down to check for either a obstacle or terrain
                else if(Physics.Raycast(pos, Vector3.down, out rh, 200))
                {
                    // the ray hit terrain or obstacle; break outta the loop
                    if(rh.transform.CompareTag("Terrain") || rh.transform.CompareTag("Obstacle"))
                    {
                        // Make sure distance is off the terrain an amount
                        if(rh.distance <= 8.0f)
                        {
                            pos += new Vector3(0.0f, 10.0f, 0.0f);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                //ray doesnt hit anything; move pos up and loop again
                else
                {
                    pos += new Vector3(0.0f, 2.0f, 0.0f);
                }
            }
            
            // Instantiate a node and put it into the patrolNodes array.
            Transform t = Instantiate(patrolNode, pos, Quaternion.identity).transform;
            patrolNodes[i] = t;
        }

        hasInitializedPatrol = true;
    }

    // Calculates a worldspace point given a center, a radius, and an angle 
    private Vector3 CirclePosition(Vector3 center, float radius, int angle)
    {
        float a = angle;
        Vector3 pos = new Vector3(
            center.x + radius * Mathf.Sin(a * Mathf.Deg2Rad),
            center.y,
            center.z + radius * Mathf.Cos(a * Mathf.Deg2Rad)
        );
        return pos;
    }

    // Clears all the old information about the patrol path
    private void DestroyOldPatrolPath()
    {
        hasInitializedPatrol = false;
        foreach(Transform t in patrolNodes)
            Destroy(t.gameObject);
        patrolNodes = new Transform[patrolNodesCount];
    }

    private Vector3 DetermineTurnDirection()
    {
        int r = Random.Range(0, 2);
        if(r == 0)
        {
            return Vector3.up;
        }
        else
        {
            return Vector3.down;
        }
    }

    private void MoveTowardsPoint()
    {
        // If we had an old patrol path, destroy it.
        if(hasInitializedPatrol)
        {
            DestroyOldPatrolPath();
        }

        // Check if we've arrived at the point of interest.
        if(Vector3.Distance(transform.position, currentPointOfInterest.position) <= interestArriveDistance)
        {
            sm.hasArrivedAtPoint = true;
        }

        // If there is nothing in the way, move and rotate towards the point of interest
        if(!somethingIsInTheWay && !terrainTooClose)
        {
            Vector3 relativePos = currentPointOfInterest.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        }

        // Move in a forward direction
        transform.Translate(Vector3.forward * Time.deltaTime * sharkSwimmingSpeed);

        // Obstacle and terrain detection
        RaycastHit r;
        if(Physics.Raycast(transform.position, currentPointOfInterest.position - transform.position, out r))
        {
            if(!r.collider.gameObject.CompareTag("PointOfInterest"))
            {
                DetectObstaclesAndTerrain();
            }  
        }
    
        // Obstacle and terrain navigation
        NavigateObstaclesAndTerrain(currentPointOfInterest);
    }

    private void ChasePlayer()
    {
        if(!hasStartedChasing)
        {
            jumpscareSound.Play();
            hasStartedChasing = true;
            sm.hasArrivedAtPoint = false;
        }
        if(hasInitializedPatrol)
        {
            DestroyOldPatrolPath();
        }
        somethingIsInTheWay = false;
        
        Vector3 relativePos = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 6);

        transform.Translate(Vector3.forward * Time.deltaTime * sharkSwimmingSpeed * 2f);

    }

    private void NavigateObstaclesAndTerrain(Transform goal)
    {
        Vector3 direction = (goal.position - transform.position).normalized;

        if(terrainTooClose || somethingIsInTheWay)
        {
            if(Physics.Raycast(transform.position, transform.right, out obstacleHit, 2) || Physics.Raycast(transform.position, -transform.right, out obstacleHit, 2)) 
            {
                if(obstacleHit.collider.gameObject.CompareTag("Obstacle")) 
                {
                    somethingIsInTheWay = false;
                }
            }
            if(Physics.Raycast(transform.position, direction, out obstacleHit))
            {
                if(obstacleHit.collider.gameObject.CompareTag("Terrain") || obstacleHit.collider.gameObject.CompareTag("Obstacle"))
                {
                    terrainTooClose = true;
                    somethingIsInTheWay = true;
                }
                else
                {
                    terrainTooClose = false;
                    somethingIsInTheWay = false;
                }
            }
        }
    }

    private void DetectObstaclesAndTerrain()
    {
        if(Physics.SphereCast(transform.position, 1f, transform.forward, out obstacleHit, obstacleDetectionDistance))
        {
            if(obstacleHit.collider.gameObject.CompareTag("Obstacle"))
            {
                if(!somethingIsInTheWay)
                {
                    turnDirection = DetermineTurnDirection();
                }
                somethingIsInTheWay = true;
                transform.Rotate(turnDirection * Time.deltaTime * obstacleAvoidanceRotationSpeed);
            }
            else if(obstacleHit.collider.gameObject.CompareTag("Terrain"))
            {
                Debug.Log("PULL UP SHARK!!");
                transform.Rotate(Vector3.left * Time.deltaTime * obstacleAvoidanceRotationSpeed * 10);
                terrainTooClose = true;
            }
        }
    }

    public void TakeDamageFromPlayer()
    {
        sm.isAfraidOfPlayer = true;
        Transform oldPoint = Instantiate(patrolNode, transform.position, Quaternion.identity).transform;
        lastPointOfInterest = oldPoint;
        runToThisPoint = DetermineWhereToRunTo();
        sharkPainSFX.Play();
        if(hasInitializedPatrol)
            DestroyOldPatrolPath();  
    }

    private void RunAway()
    {
        runAwayTimer += Time.deltaTime;

        if(Vector3.Distance(transform.position, runToThisPoint.position) <= interestArriveDistance || runAwayTimer >= runAwayMaxTime)
        {
            sm.isAfraidOfPlayer = false;
            currentPointOfInterest = lastPointOfInterest;
            sm.hasArrivedAtPoint = false;
            runAwayTimer = 0.0f;
        }

        if(!somethingIsInTheWay && !terrainTooClose)
        {
            Vector3 relativePos = runToThisPoint.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        }

        transform.Translate(Vector3.forward * Time.deltaTime * sharkSwimmingSpeed * 2.5f);

        DetectObstaclesAndTerrain();

        NavigateObstaclesAndTerrain(runToThisPoint);
    }

    private Transform DetermineWhereToRunTo()
    {
        Transform[] candidates = new Transform[patrolNodesCount];
        Vector3 center = transform.position;
        
        for(int i = 0; i < patrolNodesCount; ++i)
        {
            // Generates angle in the circle that the ith node needs to be placed at
            int angle = 360 / patrolNodesCount * i;
            // pos is a given location in the imaginary circle with patrolRadius radius and at "angle" angle
            Vector3 pos = CirclePosition(center, runAwayDistance, angle);
            
            while(true)
            {
                // Determine if the node placement would intersect with another collider
                if(Physics.CheckSphere(pos, 1f))
                {
                    // Move up until we're out of the object
                    if(pos.y < 0.0f)
                    {
                        pos += new Vector3(0.0f, 1.0f, 0.0f);
                    }
                    // If we're out of room to move up, just move to the z axis
                    else
                    {
                        pos += new Vector3(0.0f, 0.0f, 1.0f);
                    }
                }
                // If there's nothing in the check sphere, then break; we're good
                else
                {
                    break;
                }
            }
            
            // Instantiate a node and put it into the patrolNodes array.
            Transform t = Instantiate(patrolNode, pos, Quaternion.identity).transform;
            candidates[i] = t;
        }

        Transform final;
        final = candidates[Random.Range(0, patrolNodesCount)];

        /*
        foreach(Transform t in candidates)
            Destroy(t.gameObject);
        */
        return final;
    }

    private void KillPlayer()
    {
        player.GetComponent<PlayerStats>().KillPlayer("Eaten by shark");
    }
}
