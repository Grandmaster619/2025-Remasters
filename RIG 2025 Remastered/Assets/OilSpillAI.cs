using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class OilSpillAI : MonoBehaviour
{
    [Header("Object References.")]
    public LayerMask EnemyLayer;    // Enemy layer reference so the raycast detection can ignore the objects that make up the monster
    [SerializeField] Transform Player;  // The player transform for reference
    public LayerMask LayerMask; // The layer mask for which objects the raycast should ignore
    public GameObject TempPlayerPosition;
    public GameObject EnemyPathPoints;
    private Health health;

    [Space]
    [Header("Changeable Variables.")]
    public float ViewDistance = 100;    // How far away can the monster detect the player
    public float SneakingViewDistance = 40f;
    public float MaxLungeDistance = 50f;
    public float ViewAngle = 80f;   // How wide of an angle in front of the monster can it detect the player
    public float LockedOnAngle = 10f;   // How wide of an angle to determine that the player is directly in front of the monster
    public float PlayerSneakAngle = 40f;
    public float NormalSpeed = 6f;
    public float LungeSpeed = 16f;
    public float LungeCooldown = 50f;
    public int EnemyDamage = 34;
    public float PlayerCollisionDistance = 4f;

    [Space]
    [Header("Mainly debug and information variables.")]
    public float DistanceToPlayer;  // The distance the monster is to the player
    public float DistanceToTemp;
    public float LungeTimer;
    public State MonsterState; // The current state the monster is in
    public enum State { FollowPlayer, FollowLastPlayerLocation, Lunge, Search, StandStill, Patrol };
    public bool PlayerLockedOn = false; // The player is directly in front of the monster
    public bool PlayerSpotted = false; // The monster is looking at the player
    public bool PlayerInView = false; // The monster and player are in viewing range of each other
    public int PatrolPoint;
    public bool PlayerHit = false;

    private AIPath AIPath;  // The AIPath script logic so the monster can pathfind to the player
    private AIDestinationSetter Destination;
    private GameObject _TempPlayerPosition;
    private int _RandomTurnDirection;
    private float _TempYRotation;
    
    

    // Start is called before the first frame update
    void Start()
    {
        AIPath = GetComponent<AIPath>();
        Destination = GetComponent<AIDestinationSetter>();
        AIPath.enabled = false;
        //MonsterState = State.StandStill;
        _TempPlayerPosition = Instantiate(TempPlayerPosition);
        health = GameObject.Find("FirstPersonPlayer").GetComponent<Health>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DistanceToPlayer = Vector3.Distance(Player.position, transform.position);
        DistanceToTemp = Vector3.Distance(TempPlayerPosition.transform.position, transform.position);

        switch (MonsterState)
        {
            case State.FollowPlayer:
                FollowPlayer();
                break;
            case State.FollowLastPlayerLocation:
                FollowLastPlayerLocation();
                break;
            case State.Lunge:
                Lunge();
                break;
            case State.Search:
                Search();
                break;
            case State.StandStill:
                StandStill();
                break;
            case State.Patrol:
                Patrol();
                break;
            default:
                Search();
                break;
        }

        PlayerHit = false;
        Vector3 direction = transform.position - Player.position;
        if (Physics.Raycast(transform.position, -direction, out RaycastHit hit, PlayerCollisionDistance, ~LayerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag("Player")) // If there is an unobstructed view from the monster to the player
            {
                Debug.DrawRay(transform.position, -direction.normalized * hit.distance, Color.magenta, 0.1f, false); // The raycast detected something
                health.TakeDamage(EnemyDamage);
                PlayerHit = true;
            }
        }
    }

    void Patrol()
    {
        PlayerRaycast(ViewDistance);
        //Debug.Log(PlayerSpotted);
        if (PlayerSpotted || PlayerHit)
        {
            Transition_FollowPlayer();
        }

        if (AIPath.reachedEndOfPath)
        {
            Transition_Search();
        }
    }

    void Search()
    {
        PlayerRaycast(ViewDistance);
        //Debug.Log(PlayerSpotted);
        if (PlayerSpotted || PlayerHit)
        {
            Transition_FollowPlayer();
        }
        float rotate = _RandomTurnDirection * AIPath.rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotate , 0);
        float rotationDifference = transform.eulerAngles.y - _TempYRotation;
        //Debug.Log(transform.rotation.y + " " + _TempYRotation + " " + rotationDifference);
        if(Mathf.Abs(rotationDifference) < 0.05 )
        {
            Transition_Patrol();
        }
    }

    void FollowPlayer()
    {
        PlayerRaycast(ViewDistance);
        if (!PlayerInView)
        {
            Invoke(nameof(Transition_FollowLastPlayerPosition), 0.3f);
        }
        else if (PlayerLockedOn && DistanceToPlayer < MaxLungeDistance)
        {
            Transition_Lunge();
        }
    }

    void FollowLastPlayerLocation()
    {
        PlayerRaycast(ViewDistance);
        if (PlayerSpotted)
        {
            Transition_FollowPlayer();
        }
        if(AIPath.reachedEndOfPath)
        {
            Transition_Search();
        }
    }

    void Lunge()
    {
        if (LungeTimer <= 0)
            Transition_FollowPlayer();
        LungeTimer--;
    }

    void StandStill()
    {
        PlayerRaycast(ViewDistance);
        if (PlayerSpotted)
        {
            //Transition_FollowPlayer();
        }
    }

    void Transition_Patrol()
    {
        AIPath.maxSpeed = NormalSpeed;
        AIPath.enabled = true;
        PatrolPoint = Random.Range(0, EnemyPathPoints.transform.childCount - 1);
        Destination.target = EnemyPathPoints.transform.GetChild(PatrolPoint).transform;
        MonsterState = State.Patrol;
    }

    void Transition_Search()
    {
        AIPath.enabled = false;
        _RandomTurnDirection = Random.Range(0, 1);
        if (_RandomTurnDirection == 0)
            _RandomTurnDirection = -1;
        _TempYRotation = transform.eulerAngles.y;
        transform.Rotate(0, _RandomTurnDirection * 8, 0);
        MonsterState = State.Search;
    }

    void Transition_FollowPlayer()
    {
        AIPath.maxSpeed = NormalSpeed;
        AIPath.enabled = true;
        Destination.target = Player;
        MonsterState = State.FollowPlayer;
    }

    void Transition_FollowLastPlayerPosition()
    {
        AIPath.maxSpeed = NormalSpeed;
        AIPath.enabled = true;
        TempPlayerPosition.transform.position = Player.position;
        Destination.target = TempPlayerPosition.transform;
        MonsterState = State.FollowLastPlayerLocation;
    }

    void Transition_Lunge()
    {
        LungeTimer = LungeCooldown;
        AIPath.maxSpeed = LungeSpeed;
        Destination.target = Player;
        MonsterState = State.Lunge;
    }

    // Shoots a raycast from the monster to the player that is blocked by walls and other obstacles.
    // Returns the object that collides with the raycast.
    // PlayerSpotted returns True if the Player is detected within the viewing angle in front of the monster.
    // PlayerInView returns True if there is an unobstructed view from the monster to the player.
    // PlayerLockedOn returns True if the player is directly in front of the monster
    RaycastHit PlayerRaycast(float distance)
    {
        PlayerSpotted = false;
        PlayerInView = false;
        PlayerLockedOn = false;
        Vector3 direction = transform.position - Player.position;
        float angle = Vector3.Angle(-direction, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.red, 0.1f);
        //Debug.Log(angle);
        if (Physics.Raycast(transform.position, -direction, out RaycastHit hit, distance, ~LayerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(transform.position, -direction.normalized * hit.distance, Color.yellow, 0.1f, false); // The raycast detected something
            //Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("Player")) // If there is an unobstructed view from the monster to the player
            {
                Debug.DrawRay(transform.position, -direction.normalized * hit.distance, Color.green, 0.1f, false);
                PlayerInView = true;
                if (hit.collider.GetComponent<PlayerMovement>().IsSneaking()) 
                {
                    if (angle < PlayerSneakAngle && hit.distance < SneakingViewDistance)
                        MonsterDetectPlayer(angle, direction, hit);
                }
                else if (angle < ViewAngle) // If the monster can detect the player in its viewing angle
                {
                    MonsterDetectPlayer(angle, direction, hit);
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, -direction.normalized * hit.distance, Color.red, 0.1f, false); // The raycast didn't hit anything
        }
        return hit;
    }

    void MonsterDetectPlayer(float angle, Vector3 direction, RaycastHit hit)
    {
        Debug.DrawRay(transform.position, -direction.normalized * hit.distance, Color.cyan, 0.1f, false);
        PlayerSpotted = true;
        if (angle < LockedOnAngle)
        {
            PlayerLockedOn = true;
        }
    }
}

