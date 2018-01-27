using UnityEngine;

/// <summary>
/// Class used to manage the game
/// </summary>
public class GameManager : Singleton<GameManager> {

    #region Public_Attributes

	public int _illPlayerForTest;
	public GameObject _charactersBodyPrefab;
	public float _SecondsForGame;
	public GameObject _scoringPrefab;

    #endregion

    #region Private_Attributes

    private ENUM_GAMESTATE _gameState;
	private ENUM_ILLNESS _illness;

	Illness _game;
	GameObject[] _players;
	Vector3[] _spawnPositions;
	
	float _timeBeforeEndOfTheRound;
	TextMesh _chronometerRenderer;

	public GameObject _ScoringRecap;

	string _pathForIllnessMaterial;

	public GameObject[] AllPlayers {
		get {return _players;}
	}

    #endregion

    #region Unity_Methods
	
	void Awake() {
		_game = gameObject.AddComponent<Cold>();
		_illness = ENUM_ILLNESS.COLD;
		ChangeGameState(ENUM_GAMESTATE.LOADING);
		_timeBeforeEndOfTheRound = _SecondsForGame;
		_chronometerRenderer = GameObject.FindGameObjectWithTag(Tags._chronometer).GetComponent<TextMesh>();
	}	

	void Update(){
		
		if (_gameState == ENUM_GAMESTATE.PLAYING) {
			if (_game.IsGameFinished () || _timeBeforeEndOfTheRound < 1f) {
				ChangeGameState (ENUM_GAMESTATE.END);
			} else {
				UpdateChronometer();
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

		case ENUM_GAMESTATE.PLAYING:
			SetAllPlayersMovementAllowance (true);
			break;

		case ENUM_GAMESTATE.END:
			SetAllPlayersMovementAllowance (false);
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
			player = Instantiate(_charactersBodyPrefab);
			player.name = "Player" + (i+1).ToString();

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

			Instantiate(_scoringPrefab).transform.SetParent(_ScoringRecap.transform);

		}
	}

	public void ContaminedPlayer(GameObject p_player) {
		foreach(GameObject player in _players) {
			if (player == p_player) {
				player.GetComponent<Player>().SetIsContamined(true, _pathForIllnessMaterial);
				ResetPlayerAction(player);
				return;
			}
		}
	}

	void ResetPlayerAction(GameObject p_player) {
		switch (_illness) {
			case ENUM_ILLNESS.COLD:
				p_player.AddComponent<ColdAction>().SetActionKey(p_player.GetComponent<Player>().PlayerNumber);
				break;

		default:
			Debug.LogError("--- Action for illness " + _illness + " not set");
			break;
		}
	}

	void UpdateChronometer() {
		_timeBeforeEndOfTheRound -= Time.deltaTime;

		string minutes = "";
		string seconds = "";

		if (_timeBeforeEndOfTheRound / 60 < 10) {
			minutes = "0";
		}

		if (_timeBeforeEndOfTheRound % 60 < 10) {
			seconds = "0";
		}

		minutes += Mathf.Floor(_timeBeforeEndOfTheRound / 60);
		seconds += Mathf.Floor(_timeBeforeEndOfTheRound % 60);

		_chronometerRenderer.text = minutes + ":" + seconds;
	}

	void SetAllPlayersMovementAllowance(bool p_canMove) {
		foreach(GameObject player in _players) {
			player.GetComponent<Player>()._allowInput = p_canMove;
		}
	}

    #endregion

}