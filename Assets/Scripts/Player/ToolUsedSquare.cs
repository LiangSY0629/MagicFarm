using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class ToolUsedSquare : MonoBehaviour
{
    public Tilemap tilemap;

    
    public int square;
    public bool delete;

    public GameObject player;
    PlayerCombat playerCombat;
    Vector2 mousePosition;
    List<Vector3Int> tilePosition;
    List<Vector3Int> newTilePosition;

    private void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1,0.8f,0.8f,0);
        playerCombat = player.GetComponent<PlayerCombat>();
        tilePosition = new List<Vector3Int>();
        newTilePosition = new List<Vector3Int>();
    }

    private void Update()
    {


        if (StatsManager.Instance.current_tool != StatsManager.Tools.None)
        {
            //如果正在攻击不判定；
            if (playerCombat.attack)
            {
                return;
            }

            // 如果点到了ui，返回；
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            };

            tilePosition.Clear();
            newTilePosition.Clear();

            square = StatsManager.Instance.square;

            // 获取鼠标位置，将其转换为单元格位置；
            transform.localScale = new Vector3(square, square, square);
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //鼠标位置所在的单元格坐标偏移1/2，显示在单元格中心；再乘以当前所需偏移量，保证显示在左下角；
            transform.position = tilemap.WorldToCell(mousePosition) + (tilemap.cellSize / 2) * (square % 2) + (square / 2) * tilemap.cellSize;

            float direction = Vector2.Distance(player.transform.position, mousePosition);

            GetComponent<SpriteRenderer>().color = new Color(1, 0.8f, 0.8f, 0f);

            //判断是否在角色附近，不在就变透明；
            if (direction < 1.5f * square)
            {

                GetTilePosition();


                //锄头
                if (StatsManager.Instance.current_tool == StatsManager.Tools.Hoe)
                {

                    //遍历所有位置，判断各个位置是否合适；
                    foreach (Vector3Int Position in tilePosition)
                    {
                        //使用耕地字典判断这个位置是否已经存在耕地；
                        if (!TileMapManager.Instance.tileCellDictionary.ContainsKey(Position))
                        {
                            GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.8f, 0.8f, 0.40f);
                            newTilePosition.Add(Position);
                        }

                    }

                    //只能单个删除耕地

                    if (TileMapManager.Instance.tileCellDictionary.ContainsKey(tilePosition[0]))
                    {
                        GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.35f, 0.35f, 0.40f);

                        //如果右键，就删除；
                        if (Input.GetKeyDown(KeyCode.Mouse1))
                        {
                            delete = true;
                            playerCombat.Attack(tilePosition);

                        }

                    }


                    //按下左键，根据队列创建耕地；
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        delete = false;

                        if (newTilePosition.Count > 0)
                        {
                            playerCombat.Attack(newTilePosition);
                        }
                    }

                }

                //种子
                else if (StatsManager.Instance.current_tool == StatsManager.Tools.Seed)
                {
                    //遍历所有位置，判断各个位置是否合适；
                    foreach (Vector3Int Position in tilePosition)
                    {
                        //使用耕地字典判断该位置是否存在耕地，耕地上是否存在植物；
                        if (TileMapManager.Instance.tileCellDictionary.ContainsKey(Position))
                        {
                            if (TileMapManager.Instance.tileCellDictionary[Position] == false)
                            {
                                GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.8f, 0.8f, 0.40f);
                                newTilePosition.Add(Position);
                            }

                        }

                    }

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if(newTilePosition.Count > 0)
                        {
                            playerCombat.Attack(newTilePosition);
                        }

                    }

                }


                //斧子
                else if (StatsManager.Instance.current_tool == StatsManager.Tools.Axe)
                {
                    //遍历所有位置，判断各个位置是否合适；
                    foreach (Vector3Int Position in tilePosition)
                    {
                        //使用耕地字典叠加植物状态来判断是否有植物，是否成熟；
                        if (TileMapManager.Instance.tileCellDictionary.ContainsKey(Position))
                        {
                            if (TileMapManager.Instance.tileCellDictionary[Position] == true)
                            {
                                TileMapManager.Instance.tilePlantDictionary.TryGetValue(Position, out Plant plant);

                                if (plant != null && plant.mature)
                                {
                                    newTilePosition.Add(Position);
                                    GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.8f, 0.8f, 0.40f);
                                }

                            }

                        }

                    }

                    //左键收获植物；
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (newTilePosition.Count > 0)
                        {
                            playerCombat.Attack(newTilePosition);
                        }

                    }

                }


                //水壶
                else
                {
                    //遍历所有位置，判断各个位置是否合适；
                    foreach (Vector3Int Position in tilePosition)
                    {

                        if (TileMapManager.Instance.tileCellDictionary.ContainsKey(Position))
                        {
                            
                            if (TileMapManager.Instance.tileCellDictionary[Position] == true)
                            {
                                TileMapManager.Instance.tilePlantDictionary.TryGetValue(Position, out Plant plant);

                                if (plant != null && !plant.water)
                                {
                                    
                                    newTilePosition.Add(Position);
                                    GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.8f, 0.8f, 0.40f);
                                }
                                
                            }
                            
                        }
                        
                    }

                    //左键浇水；
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (newTilePosition.Count > 0)
                        {
                            playerCombat.Attack(newTilePosition);
                        }

                    }

                }

            }

        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 0.8f, 0.8f, 0);
        }
    }


    void GetTilePosition()
    {
        //根据当前区块范围，判定需要获取多少个位置；
        for (int i = 0; i < square; i++)
        {
            for (int j = 0; j < square; j++)
            {
                Vector2 position = mousePosition + new Vector2(i, j);

                tilePosition.Add(tilemap.WorldToCell(position));
              
            }

        }
    }

}