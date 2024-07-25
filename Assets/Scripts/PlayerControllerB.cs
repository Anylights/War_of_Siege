using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerControllerB : MonoBehaviour
{
    public string currentState;
    public MapManager mapManager;
    public PlayerControllerA AnotherPlayer;
    public GameObject moveIndicatorPrefab;
    public int startPosX;
    public int startPosZ;
    public AnimationCurve bounceCurve; // 用于弹跳效果的曲线
    public AnimationCurve scaleCurve;  // 用于scale效果的曲线

    public AudioSource audioSource;

    public AudioClip BounceClip;
    public AudioClip WallClip;
    public AudioClip BigBounceClip;


    private GameObject moveIndicator;
    private Vector2 indicatorPos;
    private Vector2 moveInput;
    private List<Vector2> path;
    private Coroutine moveIndicatorCoroutine;

    private GameObject downWall;
    private GameObject upWall;
    private GameObject leftWall;
    private GameObject rightWall;
    private GameObject activeWall;


    private void Start()
    {
        SetState("Wait");
        moveInput = Vector2.zero;
        indicatorPos = new Vector2(startPosX, startPosZ);
        transform.position = mapManager.GetMap()[startPosX, startPosZ].transform.position;
        moveIndicator = Instantiate(moveIndicatorPrefab, transform.position, Quaternion.identity);
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleInput();
    }

    public void SetState(string newState)
    {
        currentState = newState;

        if (currentState == "Move")
        {
            // 初始化路径
            path = new List<Vector2> { indicatorPos };
            mapManager.GetMap()[Mathf.RoundToInt(indicatorPos.x), Mathf.RoundToInt(indicatorPos.y)].GetComponent<Tile>().isInPath(true);
        }

        if (currentState == "SetWall")
        {
            int nowX = Mathf.RoundToInt(indicatorPos.x);
            int nowZ = Mathf.RoundToInt(indicatorPos.y);

            // 创建墙壁
            downWall = (nowZ == 0) ? null : mapManager.AddRowWall(nowX, nowZ);
            upWall = (nowZ == mapManager.mapSizeZ - 1) ? null : mapManager.AddRowWall(nowX, nowZ + 1);
            leftWall = (nowX == 0) ? null : mapManager.AddColumnWall(nowX, nowZ);
            rightWall = (nowX == mapManager.mapSizeX - 1) ? null : mapManager.AddColumnWall(nowX + 1, nowZ);

            // 默认设置上墙为活跃墙
            SetActiveWall(upWall);
        }
    }


    private void HandleInput()
    {
        // 移动状态
        if (currentState == "Move")
        {
            Vector2 tempMoveInput = Vector2.zero;
            Vector2 real_Indicator_Pos = indicatorPos + moveInput;
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                if ((Input.GetKeyDown(KeyCode.W)) && (mapManager.WallRow[Mathf.RoundToInt(real_Indicator_Pos.x), Mathf.RoundToInt(real_Indicator_Pos.y) + 1] != 1))
                    tempMoveInput.y = 1;
                if ((Input.GetKeyDown(KeyCode.S)) && (mapManager.WallRow[Mathf.RoundToInt(real_Indicator_Pos.x), Mathf.RoundToInt(real_Indicator_Pos.y)] != 1))
                    tempMoveInput.y = -1;
                if ((Input.GetKeyDown(KeyCode.A)) && (mapManager.WallColumn[Mathf.RoundToInt(real_Indicator_Pos.x), Mathf.RoundToInt(real_Indicator_Pos.y)] != 1))
                    tempMoveInput.x = -1;
                if ((Input.GetKeyDown(KeyCode.D)) && (mapManager.WallColumn[Mathf.RoundToInt(real_Indicator_Pos.x) + 1, Mathf.RoundToInt(real_Indicator_Pos.y)] != 1))
                    tempMoveInput.x = 1;

                Vector2 potentialMoveInput = moveInput + tempMoveInput;
                Vector2 potentialNewIndicatorPos = indicatorPos + potentialMoveInput;

                int newX = Mathf.RoundToInt(potentialNewIndicatorPos.x);
                int newZ = Mathf.RoundToInt(potentialNewIndicatorPos.y);

                // Check if the position is valid and within the move range
                if (mapManager.IsValidTile(newX, newZ) && potentialNewIndicatorPos != AnotherPlayer.GetPos())
                {
                    if (path.Contains(potentialNewIndicatorPos))
                    {
                        // 如果新位置在路径中，将路径中该位置后的所有点删除
                        int index = path.IndexOf(potentialNewIndicatorPos);
                        for (int i = index + 1; i < path.Count; i++)
                        {
                            Vector2 pos = path[i];
                            mapManager.GetMap()[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)].GetComponent<Tile>().isInPath(false);
                        }
                        path.RemoveRange(index + 1, path.Count - (index + 1));
                    }
                    else
                    {
                        if (path.Count < 4)
                        {
                            // 如果路径长度小于4，将新位置加入路径
                            path.Add(potentialNewIndicatorPos);
                            mapManager.GetMap()[newX, newZ].GetComponent<Tile>().isInPath(true);
                        }
                        else
                        {
                            // 如果路径长度大于等于4，不能移动
                            return;
                        }
                    }

                    moveInput = potentialMoveInput;
                    // 使用协程进行弹跳移动
                    if (moveIndicatorCoroutine != null)
                    {
                        StopCoroutine(moveIndicatorCoroutine);
                    }
                    moveIndicatorCoroutine = StartCoroutine(MoveIndicatorBounce(moveIndicator, mapManager.GetMap()[newX, newZ].transform.position));
                    audioSource.PlayOneShot(BounceClip);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                ConfirmMove();
            }
        }

        // 放墙状态
        if (currentState == "SetWall")
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                SetActiveWall(upWall);
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                SetActiveWall(downWall);
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SetActiveWall(leftWall);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                SetActiveWall(rightWall);
            }

            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                ConfirmWall();
            }
        }
    }

    private void ConfirmMove()
    {
        foreach (Vector2 pos in path)
        {
            mapManager.GetMap()[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)].GetComponent<Tile>().isInPath(false);
        }

        indicatorPos += moveInput;
        int newX = Mathf.RoundToInt(indicatorPos.x);
        int newZ = Mathf.RoundToInt(indicatorPos.y);
        StartCoroutine(MoveIndicatorBounce(this.gameObject, mapManager.GetMap()[newX, newZ].transform.position));
        audioSource.PlayOneShot(BigBounceClip);
        moveInput = Vector2.zero;

        // 清空路径
        path.Clear();

        SetState("SetWall");
    }


    private void SetActiveWall(GameObject wall)
    {
        // 设置所有墙不可见
        mapManager.SetWallVisible(downWall, false);
        mapManager.SetWallVisible(upWall, false);
        mapManager.SetWallVisible(leftWall, false);
        mapManager.SetWallVisible(rightWall, false);

        // 设置选中的墙可见
        activeWall = wall;
        mapManager.SetWallVisible(activeWall, true);
    }

    private void ConfirmWall()
    {
        int nowX = Mathf.RoundToInt(indicatorPos.x);
        int nowZ = Mathf.RoundToInt(indicatorPos.y);

        // 删除非活跃墙
        if (downWall != activeWall)
            Destroy(downWall);
        else
            mapManager.WallRow[nowX, nowZ] = 1;

        if (upWall != activeWall)
            Destroy(upWall);
        else
            mapManager.WallRow[nowX, nowZ + 1] = 1;

        if (leftWall != activeWall)
            Destroy(leftWall);
        else
            mapManager.WallColumn[nowX, nowZ] = 1;

        if (rightWall != activeWall)
            Destroy(rightWall);
        else
            mapManager.WallColumn[nowX + 1, nowZ] = 1;


        // 切换到下一状态（假设为Wait状态）
        SetState("Wait");
        AnotherPlayer.SetState("Move");
        audioSource.PlayOneShot(WallClip);
    }

    public Vector2 GetPos()
    {
        return indicatorPos;
    }

    // 协程实现弹跳运动
    private IEnumerator MoveIndicatorBounce(GameObject indicator, Vector3 targetPos)
    {
        Vector3 startPos = indicator.transform.position;

        float duration = 0.5f; // 持续时间
        float elapsed = 0f;
        float height = 3.0f;

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float yOffset = bounceCurve.Evaluate(t) * height; // 使用动画曲线来控制y偏移
            float x = Mathf.Lerp(startPos.x, targetPos.x, t);
            float y = Mathf.Lerp(startPos.y, targetPos.y, t) + yOffset;
            float z = Mathf.Lerp(startPos.z, targetPos.z, t);
            indicator.transform.position = new Vector3(x, y, z);

            // 调整scale
            float scaleMultiplier = scaleCurve.Evaluate(t); // 使用动画曲线来控制scale
            indicator.transform.localScale = new Vector3(1500, 1500 * scaleMultiplier, 1500);
            yield return null;
        }

        indicator.transform.position = targetPos; // 确保最后位置精确对齐
        indicator.transform.localScale = Vector3.one * 1500; // 确保最终scale恢复

    }
    public void Die()
    {
        gameObject.SetActive(false);
        moveIndicator.SetActive(false);
    }

}
