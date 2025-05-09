using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class Trap : MonoBehaviour
{
    private Animator _animator;

    public bool isClap;

    public GameObject animal;

    private void Start()
    {
        isClap = false;
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.Animal && !isClap)
        {
            _animator.SetBool("IsClap", true);
            isClap = true;

            var animalAgent = other.GetComponent<NavMeshAgent>();
            animalAgent.speed = 0f;
            animal = other.gameObject;
            var animalComponent = other.GetComponent<Animal>();
            animalComponent.IsClaped = true;
        }
        else if (other.tag == Tags.Player && (isClap == false || !animal.activeInHierarchy))
        {
            var player = other.GetComponent<Player>();
            player.TrapCount += 1;
            Destroy(gameObject);
        }
    }
}
