using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Animal : Entity
{
    public enum AnimalType
    {
        Peaceful,
        Predator,
    }

    [SerializeField] private float _minChangeTargetDistance;

    [SerializeField] private Animator _animator;

    [SerializeField] private float playerSeeDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackDelay;

    [SerializeField] private AnimalType _type = AnimalType.Peaceful;

    public bool IsClaped;

    private NavMeshAgent _agent;

    private Vector3 _targetPosition;

    private bool seePlayer;

    bool isAttacking;

    public void Init()
    {
        _agent = GetComponent<NavMeshAgent>();

        SetRandomTarget();
    }

    private void Update()
    {
        var playerToAnimalDistance = Vector3.Distance(transform.position, Player.Instance.transform.position);

        if (playerToAnimalDistance < playerSeeDistance && !Player.Instance.IsDead)
        {
            if (_type == AnimalType.Peaceful)
            {
                if (!seePlayer)
                {
                    SetRandomTarget();
                }

                seePlayer = true;

                if (!IsClaped)
                {
                    _agent.speed = 10f;
                }
            }
            else if (_type == AnimalType.Predator)
            {
                if (playerToAnimalDistance < attackDistance && isAttacking == false)
                {
                    isAttacking = true;
                    _animator.SetTrigger("Attack");
                    StartCoroutine(Attack());

                    Player.Instance.ApplyDamage(1);
                }

                _agent.SetDestination(Player.Instance.transform.position);

                seePlayer = true;
                
                if (!IsClaped)
                {
                    _agent.speed = 6f;
                }
            }
        }
        else
        {
            _agent.SetDestination(_targetPosition);
            seePlayer = false;
            
            if (!IsClaped)
            {
                _agent.speed = 2f;
            }
        }

        var Aposition = transform.position;
        Aposition.y = 0;

        var Bposition = _targetPosition;
        Bposition.y = 0;

        if (Vector3.Distance(Aposition, Bposition) < _minChangeTargetDistance)
        {
            SetRandomTarget();
        }

        if (_animator)
        {
            _animator.SetBool("IsRun", _agent.speed > 0.1f);
        }
    }

    public void SetTarget(Vector3 target)
    {
        _targetPosition = target;
        _agent.SetDestination(target);
    }

    public void SetRandomTarget()
    {
        SetTarget(Helper.ÑhoiceArray(GameManager.Init.AnimalWalkPoints).position);
    }


    public IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    protected override void PlayerOnDead()
    {
        _animator.SetBool("IsDead", true);
        gameObject.SetActive(false);
        GameManager.Init.AnimalCount -= 1;
    }
}
