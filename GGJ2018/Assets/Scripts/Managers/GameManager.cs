using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class used to manage the game
/// </summary>
public class GameManager : Singleton<GameManager> {

    #region Public_Attributes

	public int _illPlayerForTest;
	public float _SecondsForGame;
	public GameObject _scoringPrefab;
	public GameObject _charactersBodyPrefab;
	public GameObject _ScoringRecap;

    #endregion

    #region Private_Attributes

    private ENUM_GAMESTATE _gameState;
	private ENUM_ILLNESS _illness;

	Illness _game;
	GameObject[] _players;
	Vector3[] _spawnPositions;
	Text[] _playerScore;

	float _timeBeforeEndOfTheRound;
	TextMesh _chronometerRenderer;

	int _winner;
	GameObject _finalScorePanel;
	GameObject _cameraForScoring;

	public GameObject[] AllPlayers {
		get {return _players;}
	}

	public ENUM_GAMESTATE GameState {
		get { return _gameState; }
	}

    #endregion

    #region Unity_Methods
	
	void Awake() {
		_game = gameObject.AddComponent<Cold>();
		_illness = ENUM_ILLNESS.COLD;
		ChangeGameState(ENUM_GAMESTATE.LOADING);
		_timeBeforeEndOfTheRound = _SecondsForGame;
		_chronometerRenderer = GameObject.FindGameObjectWithTag(Tags._chronometer).GetComponent<TextMesh>();
		_finalScorePanel = GameObject.FindGameObjectWithTag(Tags._finalScorePanel);
		_cameraForScoring = GameObject.FindGameObjectWithTag(Tags._cameraForScoring);
	}	

	void Start() {
		_finalScorePanel.SetActive (false);
		SetAllScoreElementsTransparency (0);
	}

	void Update(){
		
		if (_gameState == ENUM_GAMESTATE.PLAYING) {
			if (_game.IsGameFinished () || _timeBeforeEndOfTheRound < 1f) {
				ChangeGameState (ENUM_GAMESTATE.END);
			} else {
				UpdateChronometer();
				UpdateScore ();
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
			SetWinnerAndScore ();
			SetCameraForWinner ();
			ChangeGameState(ENUM_GAMESTATE.SCORING);
			break;

		case ENUM_GAMESTATE.SCORING:
			StartCoroutine(FadeInScorePanel());
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
		_playerScore = new Text[4];

		for (int i = 0; i < 4; ++i) {
			GameObject player;
			player = Instantiate(_charactersBodyPrefab);
			player.name = "Player" + (i+1).ToString();

			Player p = player.AddComponent<Player>();
			p.PlayerNumber = i + 1;
			player.AddComponent<PlayerController>();
			
			player.AddComponent<Pusher>();

			p.SetIsContamined(false);

			player.transform.position = _spawnPositions[i];

			if (i == _illPlayerForTest) {
					player.AddComponent<ColdAction>();
			} else {
				player.AddComponent<NotContaminedAction>();
			}

			player.GetComponent<PlayerActions>().SetActionKey(i + 1);

			GameObject scoring = new GameObject ();
			scoring = Instantiate (_scoringPrefab);
			scoring.transform.SetParent(_ScoringRecap.transform);

			_players[i] = player;
			_playerScore [i] = scoring.GetComponent<Text> ();
		}

		_players[0].GetComponent<Player>().SetMaterial(ResourcesData._player1Material);
		_players[1].GetComponent<Player>().SetMaterial(ResourcesData._player2Material);
		_players[2].GetComponent<Player>().SetMaterial(ResourcesData._player3Material);
		_players[3].GetComponent<Player>().SetMaterial(ResourcesData._player4Material);

		_players[_illPlayerForTest].GetComponent<Player>().SetIsContamined(true);
		_players [_illPlayerForTest].GetComponent<Player> ().Score = 3 * 1064;
	}

	void UpdateScore() {
		int numberOfInfected = 0;

		//Detects the number of contamined player
		for (int i = 0; i < _players.Length; ++i) {
			if (GetPlayer(i)._isContamined) {
				numberOfInfected++;
			}
		}

		//Update the score of the pure players
		for (int i = 0; i < _players.Length; ++i) {
			Player player = GetPlayer(i);
			if (!player._isContamined) {
				player.Score += 1 * numberOfInfected * 2;
			}
		}

		for (int i = 0; i < _players.Length; ++i) {
			Player player = GetPlayer(i);
			//Debug.Log ("--- new score[" + player.PlayerNumber + "] : " + player.Score);
		}

		//Display score
		for (int i = 0; i < _players.Length; ++i) {
			_playerScore [i].text = GetPlayer(i).Score.ToString();
		}

	}

	public void ContaminedPlayer(GameObject p_player) {
		foreach(GameObject player in _players) {
			if (player == p_player) {
				player.GetComponent<Player>().SetIsContamined(true);
				ResetPlayerAction(player);
				return;
			}
		}
	}

	Player GetPlayer(int p_index) {
		return _players [p_index].GetComponent<Player>();
	}

	public void UpdateScoreSickPlayer(int p_index) {
		GetPlayer(p_index).Score += Mathf.Floor(_timeBeforeEndOfTheRound * 10);
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

	void SetAllScoreElementsTransparency(float p_value) {
		Color color;
		foreach (RawImage image in _finalScorePanel.GetComponentsInChildren<RawImage>()) {
			color = image.color;
			color.a = p_value;
			image.color = color;
		}

		foreach (Image image in _finalScorePanel.GetComponentsInChildren<Image>()) {
			image.color = new Color (1f, 1f, 1f, p_value);
		}

		foreach (Text text in _finalScorePanel.GetComponentsInChildren<Text>()) {
			text.color = new Color (0f, 0f, 0f, p_value);
		}
	}

	void SetWinnerAndScore() {
		int indexWinner = 0;
		float score = _players [indexWinner].GetComponent<Player> ().Score;

		for (int i = 1; i < _players.Length; ++i) {
			float currentPlayerScore = _players[i].GetComponent<Player> ().Score;
			Debug.Log (i + ": " + currentPlayerScore + " / " + score);

			if (currentPlayerScore > score) {
				score = currentPlayerScore;
				indexWinner = i;
			}
		}
		_winner = indexWinner;
		Debug.Log ("---Winner: " + indexWinner);
		foreach (Text text in _finalScorePanel.GetComponentsInChildren<Text>()) {
			if (text.gameObject.name == "WinnerScore") {
				text.text = "Score: " + score;
			}
		}
	}

	void SetCameraForWinner() {
		GameObject winner = _players [_winner];
		_cameraForScoring.transform.SetParent (winner.transform);
		_cameraForScoring.transform.localPosition = VectorData._cameraAvatarPosition;
		_cameraForScoring.transform.localEulerAngles = VectorData._cameraAvatarEuler;
	}

	IEnumerator FadeInScorePanel() {
		float timeForFadeIn = 2f;
		float current = 0f;
		_finalScorePanel.SetActive (true);

		while (current < timeForFadeIn) {
			float transparency = Mathf.Lerp (0, 1, current / timeForFadeIn);
			SetAllScoreElementsTransparency (transparency);
			current += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
	}

    #endregion

}