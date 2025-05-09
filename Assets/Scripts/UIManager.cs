using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _taskText;
    [SerializeField] private Text _trapCount;
    [SerializeField] private GameObject birdTiredText;
    [SerializeField] private Slider _healthSlider;

    int _count = 0;

    private void Start()
    {
        _count = GameManager.Init.AnimalCount;
        _healthSlider.maxValue = Player.Instance.Health;
    }

    private void Update()
    {
        if (EagleController.Instance != null)
        {
            if (EagleController.Instance.gameObject.activeInHierarchy)
            {
                birdTiredText.SetActive(EagleController.Instance.IsTired);
            }
            else
            {
                birdTiredText.SetActive(false);
            }
        }

        _healthSlider.value = Player.Instance.Health;

        _trapCount.text = $"{Player.Instance.TrapCount} қаппан";

        if (GameManager.Init.AnimalCount == 0)
        {
            _taskText.text = "Жарайсың, үйге оралып, аулауыңды көре аласың!";
        }
        else
        {
            _taskText.text = $"Барлық жануарлады ұстаныз. Калған жануарлар: {GameManager.Init.AnimalCount}/{_count}.";
        }
    }
}
