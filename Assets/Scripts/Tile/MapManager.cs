using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] map_all_inlst;
    public GameObject[,] map;
    public int mapSizeX;
    public int mapSizeZ;
    public float Distance_of_Wall_and_Floor;

    public GameObject Wall_prehab;


    public int[,] WallRow;
    public int[,] WallColumn;

    void Start()
    {
        Distance_of_Wall_and_Floor = 0.6f;
        if (map_all_inlst.Length < mapSizeX * mapSizeZ)
        {
            Debug.LogError("map_all_inlst 的长度不足以填充 map 数组。");
            return;
        }

        map = new GameObject[mapSizeX, mapSizeZ];
        int index = 0;
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeZ; z++)
            {
                if (index < map_all_inlst.Length)
                {
                    map[x, z] = map_all_inlst[index];
                    map[x, z].AddComponent<Tile>();
                    index++;
                }
            }
        }

        //初始化墙壁
        WallRow = CreateRowWall(mapSizeX, mapSizeZ + 1);
        WallColumn = CreateColumnWall(mapSizeX + 1, mapSizeZ);
    }



    public bool IsValidTile(int x, int z)
    {
        return x >= 0 && x < mapSizeX && z >= 0 && z < mapSizeZ && map[x, z] != null;
    }

    int[,] CreateRowWall(int rows, int columns)
    {
        int[,] matrix = new int[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // 设置矩阵边界为1
                if (j == 0)
                {
                    matrix[i, j] = 1;
                    Vector3 wallPostion = new Vector3(map[i, 0].transform.position.x, 0, map[i, 0].transform.position.z - Distance_of_Wall_and_Floor);
                    Instantiate(Wall_prehab, wallPostion, Quaternion.identity);
                }
                else if (j == columns - 1)
                {
                    matrix[i, j] = 1;
                    Vector3 wallPostion = new Vector3(map[i, 0].transform.position.x, 0, map[i, mapSizeZ - 1].transform.position.z + Distance_of_Wall_and_Floor);
                    Instantiate(Wall_prehab, wallPostion, Quaternion.identity);
                }
                else
                {
                    matrix[i, j] = 0;
                }
            }
        }
        return matrix;
    }

    int[,] CreateColumnWall(int rows, int columns)
    {
        int[,] matrix = new int[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // 设置矩阵边界为1
                if (i == 0)
                {
                    matrix[i, j] = 1;
                    Vector3 wallPostion = new Vector3(map[0, j].transform.position.x - Distance_of_Wall_and_Floor, 0, map[0, j].transform.position.z);
                    Instantiate(Wall_prehab, wallPostion, Quaternion.Euler(0, 90, 0));
                }
                else if (i == rows - 1)
                {
                    matrix[i, j] = 1;
                    Vector3 wallPostion = new Vector3(map[mapSizeX - 1, j].transform.position.x + Distance_of_Wall_and_Floor, 0, map[0, j].transform.position.z);
                    Instantiate(Wall_prehab, wallPostion, Quaternion.Euler(0, 90, 0));
                }
                else
                {
                    matrix[i, j] = 0;
                }
            }
        }
        return matrix;
    }

    public void SetWallVisible(GameObject obj, bool isVisible)
    {
        if (obj != null)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = isVisible;
            }
        }
    }

    public GameObject AddRowWall(int x, int z)
    {
        Vector3 wallPostion = new Vector3(map[x, z].transform.position.x, 0, ((map[x, z].transform.position.z + map[x, z - 1].transform.position.z) / 2));
        GameObject wall = Instantiate(Wall_prehab, wallPostion, Quaternion.identity);
        return wall;
    }

    public GameObject AddColumnWall(int x, int z)
    {
        Vector3 wallPostion = new Vector3((map[x, z].transform.position.x + map[x - 1, z].transform.position.x) / 2, 0, map[x, z].transform.position.z);
        GameObject wall = Instantiate(Wall_prehab, wallPostion, Quaternion.Euler(0, 90, 0));
        return wall;
    }
}
