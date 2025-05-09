using UnityEngine;
using System.Collections;

public class EagleController : MonoBehaviour
{
    private static EagleController instance;

    public static EagleController Instance => instance;

    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Player _player;
    [SerializeField] private float _flySpeed = 10f;
    [SerializeField] private float _rotationSpeed = 200f;
    [SerializeField] private float _flightHeight = 10f;
    [SerializeField] private float _minAttackDistance = 2f;
    [SerializeField] private float _heightAdjustSpeed = 5f;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _targetLayerMask;

    public bool IsTired;

    private float _horizontal;
    private bool _isAttacking;
    private GameObject _target;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        IsTired = false;
        _rigidbody.useGravity = false;
    }

    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");

        if (!_isAttacking)
        {
            Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + _horizontal * _rotationSpeed * Time.deltaTime, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        _animator.SetFloat("Speed", _flySpeed);

        if (Input.GetButtonDown("Jump") && !_isAttacking)
        {
            _isAttacking = true;
            _animator.SetTrigger("Attack");
            StartCoroutine(StopAttack(0.8f));
            TryDamageTarget();
        }

        _animator.SetBool("isAttacking", _isAttacking);
    }

    private void FixedUpdate()
    {
        if (!_isAttacking)
        {
            Vector3 nextPosition = _rigidbody.position + transform.forward * _flySpeed * Time.fixedDeltaTime;
            nextPosition = AdjustFlightHeight(nextPosition);
            _rigidbody.MovePosition(nextPosition);
        }
    }

    private Vector3 AdjustFlightHeight(Vector3 currentPosition)
    {
        if (Physics.Raycast(currentPosition + Vector3.up * 50f, Vector3.down, out RaycastHit hit, Mathf.Infinity, _groundLayerMask))
        {
            float targetHeight = hit.point.y + _flightHeight;
            currentPosition.y = Mathf.Lerp(currentPosition.y, targetHeight, Time.fixedDeltaTime * _heightAdjustSpeed);
        }
        return currentPosition;
    }

    private void TryDamageTarget()
    {
        _target = null;
        Collider[] hits = Physics.OverlapSphere(transform.position, _minAttackDistance, _targetLayerMask);

        if (hits.Length > 0)
        {
            float closestDistance = Mathf.Infinity;

            foreach (var hit in hits)
            {
                float distance = (hit.transform.position - transform.position).sqrMagnitude;

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    _target = hit.gameObject;
                }
            }

            if (_target != null)
            {
                transform.LookAt(_target.transform);
            }
        }
    }

    private IEnumerator StopAttack(float length)
    {
        yield return new WaitForSeconds(length);
        _isAttacking = false;

        if (_target != null)
        {
            var tag = _target.tag;

            if (tag == Tags.Player)
            {
                ChangeControlToPlayer();
            }
            else if (tag == Tags.Animal)
            {
                AnimalAttack();
            }
        }
    }

    private void ChangeControlToPlayer()
    {
        _player.ChangeActive(true);
    }

    public void AnimalAttack()
    {
        if (IsTired)
            return;

        if (_target.TryGetComponent(out Animal animal))
        {
            animal.ApplyDamage(1);
            IsTired = true;
        }
    }
}
