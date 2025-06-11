using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{

    [Header("Player")]
    public GameObject playerPref;


    #region Header DUNGEON LEVELS
    [Space(10)]
    [Header("DUNGEON LEVELS")]
    #endregion Header DUNGEON LEVELS

    #region Tooltip
    [Tooltip("Populate with the dungeon level scriptable objects")]
    #endregion Tooltip
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    #region Tooltip
    [Tooltip("Populate with the starting dungeon level for testing, first level = 0")]
    #endregion Tooltip
    [SerializeField] private int currentDungeonLevelListIndex = 0;

    [HideInInspector] public GameStates gameState;


    // Start is called before the first frame update
    private void Start()
    {
        gameState = GameStates.gameStarted;
    }

    // Update is called once per frame
    void Update()
    {
        HandleGameState();

        // For testing
        if (Input.GetKeyDown(KeyCode.Y))
        {
            gameState = GameStates.gameStarted;
        }
    }

    /// <summary>
    /// Handle game state
    /// </summary>
    private void HandleGameState()
    {
        // Handle game state
        switch (gameState)
        {
            case GameStates.gameStarted:
                // Play first level
                PlayDungeonLevel(currentDungeonLevelListIndex);
                gameState = GameStates.playingLevel;
                break;
        }
    }

    public void CreatePlayer(Vector2 pos)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = pos;
        Debug.Log("yaratıldı oyuncu");
    }

    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        // Build dungeon for level
        bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuiltSuccessfully)
        {
            Debug.LogError("Couldn't build dungeon from specified rooms and node graphs");
        }
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
    }
#endif
    #endregion Validation

}
