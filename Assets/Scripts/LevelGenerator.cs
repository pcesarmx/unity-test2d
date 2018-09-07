using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public static LevelGenerator sharedInstance;
    public List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();
    public Transform levelStartPoint;
    public List<LevelBlock> currentBlocks = new List<LevelBlock>();
    public LevelBlock firstBlock;
    private static float lastEndPointX; 

    public void Awake()
    {
        sharedInstance = this;
    }

    public void Start()
    {
        GenerateInitialBlocks();
    }

    public void AddLevelBlock() {
        int randomIndex = Random.Range(0, allTheLevelBlocks.Count);
        LevelBlock currentBlock;
        Vector3 spawnPossition = Vector3.zero;

        if(currentBlocks.Count == 0 ) {
            currentBlock = (LevelBlock)Instantiate(firstBlock);
            currentBlock.transform.SetParent(this.transform, false);
            spawnPossition = levelStartPoint.position;

            lastEndPointX = currentBlock.startPoint.position.x;
            //Debug.LogWarning("INIT LIMIT X POINT NOW IS => " + lastEndPointX);
        } else {
            currentBlock = (LevelBlock)Instantiate(allTheLevelBlocks[randomIndex]);
            currentBlock.transform.SetParent(this.transform, false);
            spawnPossition = currentBlocks[currentBlocks.Count - 1].exitPoint.position;
        }

        Vector3 correction = new Vector3(spawnPossition.x - currentBlock.startPoint.position.x,
                                         spawnPossition.y - currentBlock.startPoint.position.y, 0);
        //Debug.Log("spawnPossition.y "+ spawnPossition.y + "currentBlock.startPoint.position.x" + currentBlock.startPoint.position.x);

        currentBlock.transform.position = correction;
        currentBlocks.Add(currentBlock);
    }

    public void RemoveOldestLevelBlock()
    {
        //Debug.Log("RemoveOldestLevelBlock");
        // Debug.Log(currentBlocks.Count);
        LevelBlock oldest = currentBlocks[0];
        lastEndPointX = oldest.exitPoint.position.x;
        //Debug.LogWarning("LIMIT X POINT NOW IS => " + lastEndPointX);
        currentBlocks.Remove(oldest);
        Destroy(oldest.gameObject);

    }

    public void RemoveAllTheBlocks()
    {
        while (currentBlocks.Count>0) {
            RemoveOldestLevelBlock();
        }
    }

    public void GenerateInitialBlocks()
    {
        for (int i = 0; i < 2; i++) {
            AddLevelBlock();
        }
    }

    public float getStartXPosOldestLevel() {
        return lastEndPointX + 1;
    }
}
