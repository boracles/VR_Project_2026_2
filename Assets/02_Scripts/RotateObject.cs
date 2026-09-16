using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Inspector에서 조절할 초당 회전 각도
    [SerializeField] private float rotationSpeed = 45f;

    // Play Mode가 시작될 때 한 번 실행
    private void Start()
    {
        Debug.Log("RotateObject started.");
    }

    // Play Mode 동안 매 프레임 반복 실행
    private void Update()
    {
        transform.Rotate(
            0f,
            rotationSpeed * Time.deltaTime,
            0f
        );
    }
}