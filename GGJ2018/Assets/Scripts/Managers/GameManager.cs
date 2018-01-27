using UnityEngine;

/// <summary>
/// Class used to manage the game
/// </summary>
public class GameManager : Singleton<GameManager> {

    #region Public_Attributes

	public int _illPlayerForTest;

    #endregion

    #region Private_Attributes

    private ENUM_GAMESTATE _gameState;
	Illness _game;
	GameObject[] _players;
	Vector3[] _spawnPositions;

	string _pathForIllnessMaterial;

	public GameObject[] AllPlayers {
		get {return _players;}
	}

    #endregion

    #region Unity_Methods
	
	void Awake() {
		_game = gameObject.AddComponent<Cold>();
		ChangeGameState(ENUM_GAMESTATE.LOADING);
	}	

	void Update(){
		
		if (_gameState == ENUM_GAMESTATE.PLAYING) {
			if (_game.IsGameFinished()) {
				ChangeGameState(ENUM_GAMESTATE.END);
			}
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
			LoadSpawnPosition();
			LoadPlayers();
			ChangeGameState(ENUM_GAMESTATE.PLAYING);
			break;


			case ENUM_GAMESTATE.END:
			Debug.Log("/ ! \\ END behaviour for game manager", gameObject);
			ChangeGameState(ENUM_GAMESTATE.SCORING);
			break;

            default:
                Debug.Log("/ ! \\ No game state defined for the value: " + _gameState.ToString());
                break;
        }
    }

	void LoadSpawnPosition() {

		_spawnPositions = new Vector3[4];
		int index = 0;

		foreach(HookForSpawn spawn in FindObjectsOfType<HookForSpawn>()) {
			_spawnPositions[index] = spawn.transform.position;
			Destroy(spawn.gameObject);
			index++;
		}
	}

	void LoadPlayers() {
		_players = new GameObject[4];
		_pathForIllnessMaterial = ResourcesData._coldMaterial;
		for (int i = 0; i < 4; ++i) {
			GameObject player;
			player = GameObject.CreatePrimitive(PrimitiveType.Cube);
			player.name = "Player" + (i+1).ToString();
			Rigidbody rigidbody = player.AddComponent<Rigidbody>();
			rigidbody.isKinematic = false;

			Player p = player.AddComponent<Player>();
			p.PlayerNumber = i + 1;

			player.AddComponent<Pusher>();

			p.SetIsContamined(false);

			player.transform.position = _spawnPositions[i];

			if (i == _illPlayerForTest) {
				p.SetIsContamined(true, _pathForIllnessMaterial);
					player.AddComponent<ColdAction>();
			} else {
				player.AddComponent<NotContaminedAction>();
			}

			player.GetComponent<PlayerActions>().SetActionKey(i + 1);
			player.AddComponent<PlayerController>();
			_players[i] = player;
		}
	}

	public void ContaminedPlayer(GameObject p_player) {
		foreach(GameObject player in _players) {
			if (player == p_player) {
				player.GetComponent<Player>().SetIsContamined(true, _pathForIllnessMaterial);
				return;
			}
		}
	}

    #endregion

}