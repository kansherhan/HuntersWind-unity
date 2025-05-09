using UnityEngine;

[RequireComponent(typeof(Animator), typeof(ThirdPersonController))]
public class Player : Entity
{
    private static Player _instance;

    public static Player Instance => _instance;

    [SerializeField] private GameObject _bird;
    [SerializeField] private CameraController _cameraController;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject deadPanel;

    [SerializeField] private GameObject TrapPrefab;
    [SerializeField] private LayerMask _groundLayerMask;

    private ThirdPersonController _thirdPersonController;
    private Animator _animator;

    public int TrapCount;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsDead)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_thirdPersonController.IsIdle)
            {
                ChangeActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (TrapCount > 0)
            {
                if (Physics.Raycast(transform.position + (Vector3.up * 5f) + (transform.forward * 3f), Vector3.down, out RaycastHit hit, 1000f, _groundLayerMask))
                {
                    Instantiate(TrapPrefab, hit.point, Quaternion.identity);
                    TrapCount--;
                }
            }
        }
    }

    public void ChangeActive(bool active)
    {
        _thirdPersonController.enabled = active;
        enabled = active;

        if (!active)
        {
            _bird.SetActive(true);
            _cameraController.Player = _bird.transform;
            var birdPosition = transform.position;
            birdPosition.y = 10f;
            _bird.transform.position = birdPosition;
            _bird.GetComponent<EagleController>().IsTired = false;

            string[] animatorParams = new []{ "run", "air", "sprint", "crouch" };
            foreach (var item in animatorParams)
            {
                _animator.SetBool(item, false);
            }
        }
        else
        {
            _bird.SetActive(false);
            _cameraController.Player = transform;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.Home) && GameManager.Init.AnimalCount == 0)
        {
            winPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    protected override void PlayerOnDead()
    {
        _animator.SetTrigger("dead");
        deadPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
