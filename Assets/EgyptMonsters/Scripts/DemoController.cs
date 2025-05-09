using UnityEngine;
using System.Collections;
using System.Linq;

public class DemoController : MonoBehaviour
{
	[SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private Player _player;

	public float walkspeed = 5;
	private float horizontal;
	private float vertical;
	private float rotationDegreePerSecond = 300;
	private bool isAttacking = false;

    public GameObject[] targets;
    public float minAttackDistance;

	private void Start()
	{
        targets = GameManager.Init.WorldAnimals.Select(animal => animal.gameObject).Concat(new[] { _player.gameObject }).ToArray();
	}

	private void FixedUpdate()
	{
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 stickDirection = new Vector3(horizontal, 0, vertical);
        float speedOut;

        if (stickDirection.sqrMagnitude > 1) stickDirection.Normalize();

        if (!isAttacking)
            speedOut = stickDirection.sqrMagnitude;
        else
            speedOut = 0;

        if (stickDirection != Vector3.zero && !isAttacking)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(stickDirection, Vector3.up), rotationDegreePerSecond * Time.deltaTime);

        _rigidbody.velocity = transform.forward * speedOut * walkspeed + new Vector3(0, _rigidbody.velocity.y, 0);

        _animator.SetFloat("Speed", speedOut);
    }

	private void Update()
	{
        if (Input.GetButtonDown("Jump") && !isAttacking)
        {
            isAttacking = true;
            _animator.SetTrigger("Attack");
            StartCoroutine(stopAttack(1.5f));
            tryDamageTarget();
        }

        _animator.SetBool("isAttacking", isAttacking);
    }

    private void ChangeControlToPlayer()
    {
        _player.ChangeActive(true);
    }

    GameObject target = null;
    public void tryDamageTarget()
    {
        target = null;
        float targetDistance = minAttackDistance + 1;

        foreach (var item in targets)
        {
            if (item == null)
                continue;

            float itemDistance = (item.transform.position - transform.position).magnitude;

            if (itemDistance < minAttackDistance)
            {
                if (target == null) {
                    target = item;
                    targetDistance = itemDistance;
                }
                else if (itemDistance < targetDistance)
                {
                    target = item;
                    targetDistance = itemDistance;
                }
            }
        }

        if (target != null)
        {
            transform.LookAt(target.transform);
        }
    }
    
    public void DealDamage(DealDamageComponent comp)
    {
        if (target != null)
        {
            if (!target.CompareTag("Player"))
            {
                if (target.TryGetComponent(out Animal animal))
                {
                    Debug.Log(animal.gameObject.name);
                    animal.ApplyDamage(1);
                }
            }
        }
    }

    public IEnumerator stopAttack(float length)
	{
		yield return new WaitForSeconds(length); 
		isAttacking = false;

        if (target != null && target.CompareTag("Player"))
        {
            ChangeControlToPlayer();
        }
	}
}
