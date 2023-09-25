using Enum;
using System.Collections.Generic;
using UnityEngine;

public class BattlePositionComponent : Component
{
    [SerializeField] public List<GameObject> heroLine = new List<GameObject>();
    [SerializeField] public List<GameObject> enemyLine = new List<GameObject>();

    [SerializeField] public List<GameObject> heroStartLine = new List<GameObject>();
    [SerializeField] public List<GameObject> enemyStartLine = new List<GameObject>();

    [SerializeField] public GameObject maxYGo = null;
    [SerializeField] public GameObject minYGo = null;

    private float maxY => maxYGo != null ? maxYGo.transform.position.y : 0;
    private float minY => minYGo != null ? minYGo.transform.position.y : 0;

    private float GetBattleLineXPos(BattleLineType position, CampEnum camp, bool isStart)
    {
        int positionIndex = (int)position;

        List<GameObject> posList = null;
        if(isStart == false)
            posList = camp == CampEnum.Hero ? heroLine : enemyLine;
        else
            posList = camp == CampEnum.Hero ? heroStartLine : enemyStartLine;

        if (positionIndex < 0 || posList.Count <= positionIndex || posList[positionIndex] == null)
        {
            Debug.LogError($"GetBattlePosition :: Not Found Postion : {position}, {camp}");
            return 0;
        }

        return posList[positionIndex].transform.position.x;
    }

    public Vector2 GetBattlePos(BattleLineType position, CampEnum camp, int[] array, int currentIndex, bool isStart)
    {
        float xPos = GetBattleLineXPos(position, camp, isStart);

        float yInterval = (maxY - minY) / (array.Length + 1);

        float yPos = maxY -(yInterval * (currentIndex + 1));

        return new Vector2(xPos, yPos);
    }
}
