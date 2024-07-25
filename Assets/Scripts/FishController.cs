using UnityEngine;

public class FishController : MonoBehaviour
{
    public Transform centerPoint; // 圆心
    public float radius = 5f;     // 圆周半径
    public float speed = 1f;      // 运动速度（角速度）

    private float angle;          // 当前角度

    void Start()
    {
        if (centerPoint == null)
        {
            Debug.LogError("中心点未设置");
        }

        // 初始角度可以随机设置，也可以全部设置为0
        angle = Random.Range(0f, 360f);
    }

    void Update()
    {
        // 计算新的角度
        angle += speed * Time.deltaTime;

        // 将角度限制在0-360度之间
        angle = angle % 360f;

        // 计算新的位置
        float x = centerPoint.position.x + radius * Mathf.Cos(angle);
        float z = centerPoint.position.z + radius * Mathf.Sin(angle);

        // 更新鱼的位置信息
        Vector3 newPosition = new Vector3(x, transform.position.y, z);
        transform.position = newPosition;

        // 计算当前切线方向
        float nextAngle = angle + speed * Time.deltaTime;
        float nextX = centerPoint.position.x + radius * Mathf.Cos(nextAngle);
        float nextZ = centerPoint.position.z + radius * Mathf.Sin(nextAngle);
        Vector3 nextPosition = new Vector3(nextX, transform.position.y, nextZ);

        // 计算方向向量
        Vector3 direction = (nextPosition - newPosition).normalized;

        // 设置鱼的旋转，使其朝向切线方向
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
    }
}
