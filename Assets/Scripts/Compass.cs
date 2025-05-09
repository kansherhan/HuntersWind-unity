using UnityEngine;

public class Compass : MonoBehaviour
{
    [Header("������ �� UI Image �������")]
    public RectTransform compassImage;

    [Header("�������� �������� ��������")]
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

        // ������ ������������� ����
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, smoothSpeed * Time.deltaTime);

        // ��������� ���� �������� � ����������� �������
        compassImage.localEulerAngles = new Vector3(0, 0, currentAngle);
    }
}
