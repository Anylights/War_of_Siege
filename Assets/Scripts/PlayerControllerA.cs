using UnityEngine;
using System.Collections.Generic;

public class PlayerControllerA : MonoBehaviour
{
    public string currentState;
    public MapManager mapManager;
    public PlayerControllerB AnotherPlayer;
    public GameObject moveIndicatorPrefab;
    public int startPosX;
    public int startPosZ;

    private GameObject moveIndicator;
    private Vector2 indicatorPos;
    private Vector2 moveInput;
    private List<Vector2> path;

    private GameObject downWall;
    private GameObject upWall;
    private GameObject leftWall;
    private GameObject rightWall;
    private GameObject activeWall;

    public void SetState(string newState)
    {
        currentState = newState;

        if (currentState == "Move")
        {
            // 初始化路径
            path = new List<Vector2> { indicatorPos };
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

    private void Start()
    {
        SetState("Move");
        moveInput = Vector2.zero;
        indicatorPos = new Vector2(startPosX, startPosZ);
        transform.position = mapManager.GetMap()[startPosX, startPosZ].transform.position;
        moveIndicator = Instantiate(moveIndicatorPrefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // 移动状态
        if (currentState == "Move")
        {
            Vector2 tempMoveInput = Vector2.zero;
            Vector2 real_Indicator_Pos = indicatorPos + moveInput;

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
                    path.RemoveRange(index + 1, path.Count - (index + 1));
                }
                else
                {
                    if (path.Count < 4)
                    {
                        // 如果路径长度小于4，将新位置加入路径
                        path.Add(potentialNewIndicatorPos);
                    }
                    else
                    {
                        // 如果路径长度大于等于4，不能移动
                        return;
                    }
                }

                moveInput = potentialMoveInput;
                moveIndicator.transform.position = mapManager.GetMap()[newX, newZ].transform.position;
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

    private void ConfirmMove()
    {
        indicatorPos += moveInput;
        int newX = Mathf.RoundToInt(indicatorPos.x);
        int newZ = Mathf.RoundToInt(indicatorPos.y);
        transform.position = mapManager.GetMap()[newX, newZ].transform.position;
        moveInput = Vector2.zero;

        // 清空路径
        path.Clear();

        SetState("SetWall");
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
    }

    public Vector2 GetPos()
    {
        return indicatorPos;
    }
}