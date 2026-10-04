using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator anim;
    public SeedChoose seedChoose;

    public bool attack;


    ToolUsedSquare toolUsed;
    PlantDictionary plantDictionary;
    PlantController plantController;
    

    private void Start()
    {
        StatsManager.Instance.current_tool = StatsManager.Tools.None;
        toolUsed = GetComponentInChildren<ToolUsedSquare>();
        plantDictionary = FindObjectOfType<PlantDictionary>();
        plantController = FindObjectOfType<PlantController>();

    }

    //判断当前使用工具，并根据工具产生相应的效果；

    public void Attack(List<Vector3Int> tilePositions)
    {
        //没用工具，返回；
        if (StatsManager.Instance.current_tool == StatsManager.Tools.None)
        {
            return;
        }
        //斧子，当范围内有植物时触发收获函数；
        else if (StatsManager.Instance.current_tool == StatsManager.Tools.Axe)
        {
            foreach(Vector3Int position in tilePositions)
            {
                if (!TileMapManager.Instance.tileCellDictionary.ContainsKey(position))
                {
                    return;
                }

                TileMapManager.Instance.tilePlantDictionary.TryGetValue(position, out Plant plant);
                if (plant != null)
                {
                    plant.HarvestCrop();
                }

                TileMapManager.Instance.tileCellDictionary[position] = false;
                TileMapManager.Instance.tilePlantDictionary.Remove(position);

            }


            anim.SetFloat("Tool", 0);
            SoundEffectManager.Instance.PlayAudio("Attack");
        }


        //锄头，当前选中地块不为耕地时，生成耕地；
        else if (StatsManager.Instance.current_tool == StatsManager.Tools.Hoe)
        {
            
            //如果不是右键删除，添加
            if (!toolUsed.delete)
            {
                foreach(Vector3Int position in tilePositions)
                {
                    if (TileMapManager.Instance.tileCellDictionary.ContainsKey(position))
                    {
                        return;
                    }

                    TileMapManager.Instance.SetTile(toolUsed.tilemap, position);
                }

            }
            else if (toolUsed.delete)
            {
                foreach(Vector3Int position in tilePositions)
                {

                    //判断是否有植物；有就带着一起删除；
                    if (!TileMapManager.Instance.tileCellDictionary.ContainsKey(position))
                    {
                        return;
                    }

                    if (TileMapManager.Instance.tileCellDictionary[position] == true)
                    {
                        TileMapManager.Instance.tilePlantDictionary.TryGetValue(position, out Plant plant);
                        if (plant != null)
                        {
                            Destroy(plant.gameObject);
                            TileMapManager.Instance.tilePlantDictionary.Remove(position);
                        }
                    }

                    TileMapManager.Instance.DeleteTile(toolUsed.tilemap, position);

                }
                
            }

            anim.SetFloat("Tool", 1);
            SoundEffectManager.Instance.PlayAudio("Attack");

        }
        // 水壶，范围内存在植物时可进行浇水；
        else if (StatsManager.Instance.current_tool == StatsManager.Tools.Kettle)
        {
            anim.SetFloat("Tool", 2);
            SoundEffectManager.Instance.PlayAudio("Water");

            if (TimeController.Instance.isNight)
            {
                TipsPopupControler.Instance.SetTipsText("拜托~ 晚上还给植物浇水，你觉得晚上它会生长吗？");
                return;
            }

            foreach (Vector3Int position in tilePositions)
            {
                TileMapManager.Instance.tilePlantDictionary.TryGetValue(position, out Plant plant);
                if (plant != null)
                {
                    plant.IsWatering();
                }
            }
            
        }

        //种子，选中地块为耕地并且不存在植物时，根据所选种子类型、种植植物；
        else if (StatsManager.Instance.current_tool == StatsManager.Tools.Seed)
        {
            //实例化植物预制体，并将字典里的东西更新；
            GameObject plantPrefab = plantDictionary.SetPlantPrefabs(seedChoose.currentSeed);
            if (plantPrefab != null)
            {
                foreach (Vector3Int position in tilePositions)
                {
                    if (!TileMapManager.Instance.tileCellDictionary.ContainsKey(position))
                    {
                        return;
                    }

                    Plant plant = Instantiate(plantPrefab, plantController.transform).GetComponent<Plant>();
                    plant.transform.position = position + toolUsed.tilemap.cellSize / 2;
                    TileMapManager.Instance.tilePlantDictionary[position] = plant;
                    TileMapManager.Instance.tileCellDictionary[position] = true;
                }
            }

            anim.SetFloat("Tool", 1);
            SoundEffectManager.Instance.PlayAudio("Attack");
        }
        anim.SetBool("Attack", true);
        attack = true;
    }

    public void FinalAttack()
    {
        anim.SetBool("Attack", false);
        attack = false;
    }
}
