using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class used to manage the game
/// </summary>
public class GameManager : Singleton<GameManager> {

    #region Public_Attributes

	public bool _isIllPlayerForcedAtBeginning;
	public bool _isSkipingLoading = false;
	public int _illPlayerForTest;
	public int _numberOfPlayersInTheGame = 4;
	public float _SecondsForGame;
	public GameObject _scoringPrefab;
	public GameObject _charactersBodyPrefab;
	public GameObject _ScoringRecap;
	public Sprite[] _playerImages; 

    #endregion

    #region Private_Attributes

    public ENUM_GAMESTATE _gameState;
	private ENUM_ILLNESS _illness;

	Illness _game;
	GameObject[] _players;
	Vector3[] _spawnPositions;
	Text[] _playerScore;

	float _timeBeforeEndOfTheRound;
	TextMesh _chronometerRenderer;

	GameObject _goalScreen;

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
		_timeBeforeEndOfTheRound = _SecondsForGame;
		_chronometerRenderer = GameObject.FindGameObjectWithTag(Tags._chronometer).GetComponent<TextMesh>();
		_finalScorePanel = GameObject.FindGameObjectWithTag(Tags._finalScorePanel);
		_cameraForScoring = GameObject.FindGameObjectWithTag(Tags._cameraForScoring);
		_goalScreen = GameObject.FindGameObjectWithTag(Tags._goalScreen);
	}	

	void Start() {
		_finalScorePanel.SetActive (false);
		SetAllScoreElementsTransparency (0);
		_goalScreen.SetActive (false);

		ChangeGameState(ENUM_GAMESTATE.LOADINGLEVEL);
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
		case ENUM_GAMESTATE.LOADINGLEVEL:
			if (_isSkipingLoading) {
				Destroy (GameObject.FindGameObjectWithTag (Tags._loadingScreen));
				ChangeGameState (ENUM_GAMESTATE.LOADING);	
			} else {
				StartCoroutine (LoadingBar ());
			}
			break;

		case ENUM_GAMESTATE.LOADING:
			_timeBeforeEndOfTheRound = _SecondsForGame;
			LoadSpawnPosition();
			LoadPlayers();
			SetAllPlayersMovementAllowance (false);
			ChangeGameState(ENUM_GAMESTATE.GOALSCREEN);
			break;

		case ENUM_GAMESTATE.GOALSCREEN:
			_goalScreen.SetActive (true);
			_goalScreen.GetComponent<WaitForInteraction> ().enabled = true;
			break;

		case ENUM_GAMESTATE.CINEMATICS:
			_goalScreen.SetActive (false);
			int indexSick = 0;

			for (int i = 0; i < _players.Length; ++i) {
				if (GetPlayer (i)._isContamined) {
					indexSick = i;
					break;
				}
			}

			SetCamera (indexSick);
			_game.LaunchCinematic (GameObject.FindGameObjectWithTag (Tags.m_mainCamera), _cameraForScoring.transform.position, _cameraForScoring.transform.eulerAngles);
			break;

		case ENUM_GAMESTATE.COUNTDOWN:
			Debug.LogError ("Do countdown behaviour");
			SetScoreImage();
			ChangeGameState (ENUM_GAMESTATE.PLAYING);
			break;

		case ENUM_GAMESTATE.PLAYING:
			_ScoringRecap.SetActive (true);			
			SetAllPlayersMovementAllowance (true);
			break;

		case ENUM_GAMESTATE.END:
			SetAllPlayersMovementAllowance (false);
			SetWinnerAndScore ();
			SetCameraForWinner ();
			ChangeGameState(ENUM_GAMESTATE.SCORING);
			break;

		case ENUM_GAMESTATE.SCORING:
			_ScoringRecap.SetActive (false);
			StartCoroutine(FadeInScorePanel());
			break;

		case ENUM_GAMESTATE.RESET:
			_finalScorePanel.SetActive (false);
			_cameraForScoring.transform.SetParent (null);

			foreach (Text score in _playerScore) {
				score.text = "0000";
			}

			foreach (GameObject player in _players) {
				Destroy (player);
			}
			ChangeGameState(ENUM_GAMESTATE.LOADING);
			break;

            default:
                Debug.Log("/ ! \\ No game state defined for the value: " + _gameState.ToString());
                break;
        }
    }

	void LoadSpawnPosition() {

		_spawnPositions = new Vector3[_numberOfPlayersInTheGame];
		int index = 0;

		foreach(HookForSpawn spawn in FindObjectsOfType<HookForSpawn>()) {
			_spawnPositions[index] = spawn.transform.position;
			index++;
		}
	}

	void LoadPlayers() {
		_players = new GameObject[_numberOfPlayersInTheGame];
		_playerScore = new Text[_numberOfPlayersInTheGame];

		int illPlayer;

		if (_isIllPlayerForcedAtBeginning) {
			illPlayer = _illPlayerForTest;
		} else {
			illPlayer = Mathf.FloorToInt (Random.Range (0, _numberOfPlayersInTheGame));
		}

		for (int i = 0; i < _numberOfPlayersInTheGame; ++i) {
			GameObject player;
			player = Instantiate(_charactersBodyPrefab);
			player.name = "Player" + (i+1).ToString();

			Player p = player.AddComponent<Player>();
			p.PlayerNumber = i + 1;
			player.AddComponent<PlayerController>();
			
			player.AddComponent<Pusher>();
			AudioSource _as = player.AddComponent<AudioSource>();
			_as.playOnAwake = false;
			_as.clip = Resources.Load("sounds/Clack_Sound") as AudioClip;

			p.SetIsContamined(false);

			player.transform.position = _spawnPositions[i];

			if (i == illPlayer) {
					player.AddComponent<ColdAction>();
			} else {
				player.AddComponent<NotContaminedAction>();
			}

			player.GetComponent<PlayerActions>().SetActionKey(i + 1);

			GameObject scoring = new GameObject ();
			scoring = Instantiate (_scoringPrefab);
			scoring.transform.SetParent(_ScoringRecap.transform, false);

			_players[i] = player;
			_playerScore [i] = scoring.GetComponent<Text> ();
		}

		_players[0].GetComponent<Player>().SetMaterial(ResourcesData._player1Material);
		_players[1].GetComponent<Player>().SetMaterial(ResourcesData._player2Material);
		_players[2].GetComponent<Player>().SetMaterial(ResourcesData._player3Material);
		_players[3].GetComponent<Player>().SetMaterial(ResourcesData._player4Material);

		_players[illPlayer].GetComponent<Player>().SetIsContamined(true);
		_players [illPlayer].GetComponent<Player> ().Score = 3 * 1064;
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
			_playerScore [i].text = /*"P"+GetPlayer(i).PlayerNumber.ToString()+": "+*/GetPlayer(i).Score.ToString();
			_playerScore [i].color = GetPlayer(i)._playerColor; 
		}

	}

	public void SetScoreImage() {

		for (int i = 0; i < _players.Length; ++i) {
			_playerScore[i].GetComponentInChildren<Image>().sprite = _playerImages[i];
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
			text.color = new Color (1f, 1f, 1f, p_value);
		}
	}

	void SetWinnerAndScore() {
		int indexWinner = 0;
		float score = _players [indexWinner].GetComponent<Player> ().Score;

		for (int i = 1; i < _players.Length; ++i) {
			float currentPlayerScore = _players[i].GetComponent<Player> ().Score;

			if (currentPlayerScore > score) {
				score = currentPlayerScore;
				indexWinner = i;
			}
		}
		_winner = indexWinner;
		foreach (Text text in _finalScorePanel.GetComponentsInChildren<Text>()) {
			if (text.gameObject.name == "WinnerScore") {
				text.text = score.ToString();
			}
		}
	}

	void SetCameraForWinner() {
		SetCamera (_winner);

	}

	void SetCamera(int p_index) {
		_cameraForScoring.transform.SetParent (_players[p_index].transform);
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

	IEnumerator LoadingBar() {
		Debug.Log ("Coroutine");
		GameObject loadingScreen = GameObject.FindGameObjectWithTag (Tags._loadingScreen);
		RectTransform bar = null;

		foreach (Image image in loadingScreen.GetComponentsInChildren<Image>()) {
			if (image.gameObject.name == "Content") {
				bar = image.gameObject.GetComponent<RectTransform>();
				break;
			}
		}

		float timeForLoading = 5f;
		float currentTime = 0f;
		float currentBeginning = bar.transform.localPosition.x;
		float arbitraryValue = -754f;
		Debug.Log (currentBeginning);

		while (currentTime < timeForLoading) {
			float newPos = Mathf.Lerp (currentBeginning, arbitraryValue, currentTime / timeForLoading);

			if (newPos > arbitraryValue) {
				newPos = arbitraryValue;
			}

			Vector3 newPosV = new Vector3 (newPos, 0f, 0f);

			bar.transform.localPosition = newPosV;
			currentTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		GameManager.Instance.ChangeGameState (ENUM_GAMESTATE.LOADING);
		Destroy (loadingScreen);
	}

    #endregion

}