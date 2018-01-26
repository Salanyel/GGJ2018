using UnityEngine;

/// <summary>
/// Class used to manage the game
/// </summary>
public class GameManager : Singleton<GameManager> {

    #region Public_Attributes

    #endregion

    #region Private_Attributes

    private ENUM_GAMESTATE m_gameState;
	Illness _game;
	int[] _players;

	public int[] AllPlayers {
		get {return _players;}
	}

    #endregion

    #region Unity_Methods
	
	void Awake() {
		_players = new int[]{0, 1, 2, 3, 4};

		_game = gameObject.AddComponent<Cold>();
	}	


	void Update(){
		_game.IsGameFinished();
	}

    #endregion

    #region Methods

    /// <summary>
    /// Change the game state and modify all behavior in consequences
    /// </summary>
    /// <param name="p_newState">New game state</param>
    public void changeGameState(ENUM_GAMESTATE p_newState)
    {
        m_gameState = p_newState;

        switch (m_gameState)
        {
            default:
                Debug.Log("/ ! \\ No game state defined for the value: " + m_gameState.ToString());
                break;
        }
    }

    #endregion

}