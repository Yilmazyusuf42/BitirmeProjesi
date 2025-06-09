using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeGraph", menuName = "Scriptable Objects/Dungeon/Room Node Graph")]

public class RoomNodeGraphSO : ScriptableObject
{
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
    [HideInInspector] public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();
    [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();



    void Awake()
    {
        LoadRoomNodeDictionary();
    }

    /// <summary>
    /// Load the room node dictionary from the room node list.
    /// </summary>
    private void LoadRoomNodeDictionary()
    {
        roomNodeDictionary.Clear();

        // Populate dictionary
        foreach (RoomNodeSO node in roomNodeList)
        {
            roomNodeDictionary[node.id] = node;
        }
    }

    /// <summary>
    /// Get room node by roomNodeType
    /// </summary>
    public RoomNodeSO GetRoomNode(RoomNodeTypeSO roomNodeType)
    {
        foreach (RoomNodeSO node in roomNodeList)
        {
            if (node.roomNodeType == roomNodeType)
            {
            return node;
            }
        }
        return null;
    }


    /// <summary>
    /// Get child room nodes for supplied parent room node
    /// </summary>
    public IEnumerable<RoomNodeSO> GetChildRoomNodes(RoomNodeSO parentRoomNode)
    {
        foreach (string childNodeID in parentRoomNode.childRoomNodeIDList)
        {
            yield return GetRoomNode(childNodeID);
        }
    }

    /// <summary>
    /// Belirtilen ID'ye göre oda düğümünü getir.
    /// </summary>
    public RoomNodeSO GetRoomNode(string roomNodeID)
    {
        // ID sözlükte varsa, ilgili oda düğümünü döndür
        if (roomNodeDictionary.TryGetValue(roomNodeID, out RoomNodeSO roomNode))
        {
            return roomNode;
        }
        // ID bulunamazsa, null döndür
        return null;
    }


#if UNITY_EDITOR

    [HideInInspector] public RoomNodeSO roomNodeToDrawLineFrom = null;
    [HideInInspector] public Vector2 linePosition;

    
    public void OnValidate()
    {
        LoadRoomNodeDictionary();
    }

    public void SetNodeToDrawConnectionLineFrom(RoomNodeSO node, Vector2 position)
    {
        roomNodeToDrawLineFrom = node;
        linePosition = position;
    }
#endif

}
