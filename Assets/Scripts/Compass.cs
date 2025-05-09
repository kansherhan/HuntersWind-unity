using UnityEngine;

public class Compass : MonoBehaviour
{
    [Header("Ссылка на UI Image компаса")]
    public RectTransform compassImage;

    [Header("Скорость плавного поворота")]
    public float smoothSpeed = 5f;

    private float currentAngle;

    void Update()
    {
        float targetAngle = 0.0f;

        if (EagleController.Instance != null && EagleController.Instance.gameObject.activeInHierarchy)
        {
            targetAngle = EagleController.Instance.transform.eulerAngles.y;
        }
        else
        {
            targetAngle = Player.Instance.transform.eulerAngles.y;
        }

        // Плавно интерполируем угол
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, smoothSpeed * Time.deltaTime);

        // Применяем угол вращения к изображению компаса
        compassImage.localEulerAngles = new Vector3(0, 0, currentAngle);
    }
}
