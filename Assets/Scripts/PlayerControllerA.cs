using UnityEngine;

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
    private Vector2 newIndicatorPos;
    private int moveRange = 3;

    private GameObject downWall;
    private GameObject upWall;
    private GameObject leftWall;
    private GameObject rightWall;
    private GameObject activeWall;

    public void SetState(string newState)
    {
        currentState = newState;

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
        transform.position = mapManager.map[startPosX, startPosZ].transform.position;
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

            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && (mapManager.WallRow[Mathf.RoundToInt(real_Indicator_Pos.x), Mathf.RoundToInt(real_Indicator_Pos.y) + 1] != 1))
                tempMoveInput.y = 1;
            if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && (mapManager.WallRow[Mathf.RoundToInt(real_Indicator_Pos.x), Mathf.RoundToInt(real_Indicator_Pos.y)] != 1))
                tempMoveInput.y = -1;
            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && (mapManager.WallColumn[Mathf.RoundToInt(real_Indicator_Pos.x), Mathf.RoundToInt(real_Indicator_Pos.y)] != 1))
                tempMoveInput.x = -1;
            if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && (mapManager.WallColumn[Mathf.RoundToInt(real_Indicator_Pos.x) + 1, Mathf.RoundToInt(real_Indicator_Pos.y)] != 1))
                tempMoveInput.x = 1;

            Vector2 potentialMoveInput = moveInput + tempMoveInput;
            Vector2 potentialNewIndicatorPos = indicatorPos + potentialMoveInput;

            int newX = Mathf.RoundToInt(potentialNewIndicatorPos.x);
            int newZ = Mathf.RoundToInt(potentialNewIndicatorPos.y);

            int distance = Mathf.RoundToInt(Mathf.Abs(potentialMoveInput.x) + Mathf.Abs(potentialMoveInput.y));

            if (mapManager.IsValidTile(newX, newZ) && distance <= moveRange)
            {
                moveInput = potentialMoveInput;
                moveIndicator.transform.position = mapManager.map[newX, newZ].transform.position;
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
        transform.position = mapManager.map[newX, newZ].transform.position;
        moveInput = Vector2.zero;
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
}
