using UnityEngine;

/// <summary>
/// Class used to manage the game
/// </summary>
public class GameManager : Singleton<GameManager> {

    #region Public_Attributes

    #endregion

    #region Private_Attributes

    private ENUM_GAMESTATE _gameState;
	Illness _game;
	Player[] _players;

	public Player[] AllPlayers {
		get {return _players;}
	}

    #endregion

    #region Unity_Methods
	
	void Awake() {
		_players = new Player[4];
		_game = gameObject.AddComponent<Cold>();

		ChangeGameState(ENUM_GAMESTATE.LOADING);
	}	

	void Update(){
		
		if (_gameState == ENUM_GAMESTATE.PLAYING) {
			_game.IsGameFinished();
		}
	}

    #endregion

    #region Methods

    /// <summary>
    /// Change the game state and modify all behavior in consequences
    /// </summary>
    /// <param name="p_newState">New game state</param>
    public void ChangeGameState(ENUM_GAMESTATE p_newState)
    {
        _gameState = p_newState;

        switch (_gameState)
        {
		
			case ENUM_GAMESTATE.LOADING:
			LoadPlayers();
			break;

            default:
                Debug.Log("/ ! \\ No game state defined for the value: " + _gameState.ToString());
                break;
        }
    }

	void LoadPlayers() {
		GameObject player;

		_players = new Player[4];

		for (int i = 0; i < 4; ++i) {
			player = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Player p = player.AddComponent<Player>();
			p.PlayerNumber = i;
			if (i == 3) {
				p.IsContamined = true;
				player.AddComponent<ColdAction>();
			} else {
				player.AddComponent<NotContamined>();
			}
		}
	}

    #endregion

}