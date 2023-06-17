using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRunGameLoop : MonoBehaviour
{
    [SerializeField] private int maxPlayers = 4;
    private List<int> players = new List<int>();

    [SerializeField] private Transform runnersSpawn;
    [SerializeField] private Transform deathSpawn;
    [SerializeField] private Transform normalSpawn;

    [SerializeField] private WebRequest webRequest;
    [SerializeField] private Barriers[] allBarriers;

    [SerializeField] private InteractionButton interactionButton;

    private bool inSession = false;
    private List<int> playersWhoNotPlayedDeathThisSession = new List<int>();
    private int playersReachedGoal;
    private Dictionary<int, Score> playerScore = new Dictionary<int, Score>();

    private float gameTime;
    private float roundTime;

    private int gameId;
    private int currentDeath;

    private void Start()
    {
        if (SessionVariables.instance.server != null) SessionVariables.instance.server.deathRunGameLoop = this;
        SessionVariables.instance.myGameClient.deathRunGameLoop = this;

        interactionButton.SubScribeAction(RequestGameStart);
    }

    // client only
    private void RequestGameStart()
    {
        SessionVariables.instance.myGameClient.SendToServer(new Net_StartGame());
    }

    // client only
    public void StartAttempt()
    {
        inSession = true;
    }

    // server only
    public void StartGame()
    {
        if (players.Count < 2)
        {
            SessionVariables.instance.server.BroadCast(new Net_ChatMessage($"Not enough players to start the round! Requires at least two players! ({players.Count}/{maxPlayers})"));
            return;
        }

        if (inSession) return;
        SessionVariables.instance.server.BroadCast(new Net_StartGame());
        inSession = true;
        gameTime = Time.time;
        playersWhoNotPlayedDeathThisSession.Clear();
        playerScore.Clear();
        foreach (int player in players)
        {
            playersWhoNotPlayedDeathThisSession.Add(player);
            playerScore.Add(player, new Score(player, 0));
        }
        playersReachedGoal = 0;
        StartCoroutine(webRequest.Request<Game>("https://studenthome.hku.nl/~yvar.toorop/php/history_add_game", (request) =>
        {
            if (request != null)
            {
                gameId = request.game_id;
                StartCoroutine(WaitForNextTurn(5));
            }
        }));
    }

    // server only
    public void NextPlayer()
    {
        if (playersWhoNotPlayedDeathThisSession.Count == 0)
        {
            EndGame();
            return;
        }

        int deathPlayer = FindNextPlayer();
        currentDeath = deathPlayer;
        foreach (var player in players)
        {
            if (player == deathPlayer) continue;
            SessionVariables.instance.server.BroadCast(new Net_TeleportPlayer(player, runnersSpawn.position.x, runnersSpawn.position.y, runnersSpawn.position.z));
        }

        SessionVariables.instance.server.BroadCast(new Net_TeleportPlayer(deathPlayer, deathSpawn.position.x, deathSpawn.position.y, deathSpawn.position.z));
        StartRound();
        SessionVariables.instance.server.BroadCast(new Net_ChatMessage("New game starting now!"));
    }

    // server only
    private int FindNextPlayer()
    {
        int random = UnityEngine.Random.Range(0, playersWhoNotPlayedDeathThisSession.Count);
        int result = playersWhoNotPlayedDeathThisSession[random];
        playersWhoNotPlayedDeathThisSession.RemoveAt(random);
        return result;
    }

    // server only
    public void StartRound()
    {
        foreach (var player in playerScore.Values) player.finished = false;
        roundTime = Time.time;
        SessionVariables.instance.server.BroadCast(new Net_OpenBarriers());
    }

    // server only
    public void EndRound()
    {
        SessionVariables.instance.server.BroadCast(new Net_CloseBarriers());
        StartCoroutine(WaitForNextTurn(5));
    }

    // client only
    public void OpenBarriers()
    {
        foreach (Barriers barrier in allBarriers)
        {
            barrier.OpenBarriers();
        }
    }

    // client only
    public void CloseBarriers()
    {
        foreach (Barriers barrier in allBarriers)
        {
            barrier.CloseBarriers();
        }
    }

    // server only
    public void EndGame()
    {
        gameTime = Time.time - gameTime;
        inSession = false;
        foreach (var player in players)
        {
            SessionVariables.instance.server.BroadCast(new Net_TeleportPlayer(player, normalSpawn.position.x, normalSpawn.position.y, normalSpawn.position.z));
        }

        foreach (var score in playerScore.Values)
        {
            StartCoroutine(webRequest.Request<Results>($"studenthome.hku.nl/~yvar.toorop/php/score_insert_score?score={score.score}&history_id={score.playerId}", null));
        }

        foreach (var player in playerScore)
        {
            Debug.Log($"{player.Key}({player.Value.playerId}): {player.Value.score}");
        }

        Score firstPlace = playerScore[players[0]];
        foreach (var score in playerScore.Values)
        {
            if (score.score < firstPlace.score)
            {
                firstPlace = score;
            }
        }

        Score secondPlace = playerScore[players[0]];
        playerScore.Remove(firstPlace.playerId);
        foreach (var score in playerScore.Values)
        {
            if (score.score < secondPlace.score)
            {
                secondPlace = score;
            }
        }

        StartCoroutine(webRequest.Request<Results>($"https://studenthome.hku.nl/~yvar.toorop/php/history_set_score?history_id={gameId}&winner_id={firstPlace.playerId}&second_id={secondPlace.playerId}&duration={gameTime}", null));
    }

    // server only
    public void ReachedGoal(int playerId)
    {
        if (!players.Contains(playerId)) return;
        if (!playerScore[playerId].finished)
        {
            playerScore[playerId].finished = true;
            playersReachedGoal++;
            if (playersReachedGoal == 2) SessionVariables.instance.server.BroadCast(new Net_ChatMessage($"{SessionVariables.instance.playerDictionary[playerId].playerName} reached the finish 2nd place!"));
            else SessionVariables.instance.server.BroadCast(new Net_ChatMessage($"{SessionVariables.instance.playerDictionary[playerId].playerName} reached the finish {playersReachedGoal}th place!"));
            if (playerId != currentDeath) playerScore[playerId].AddScore(Time.time - roundTime);
            if (playersReachedGoal >= players.Count)
            {
                EndRound();
            }
        }
    }

    // server only
    private IEnumerator WaitForNextTurn(int waitTime)
    {
        while (waitTime > 0)
        {
            SessionVariables.instance.server.BroadCast(new Net_ChatMessage($"Game starts in {waitTime}..."));
            yield return new WaitForSeconds(1);
            waitTime--;
        }
        NextPlayer();
    }

    // server only
    public void JoinPlayer(int playerId)
    {
        if (!players.Contains(playerId))
        {
            if (players.Count >= maxPlayers)
            {
                SessionVariables.instance.server.BroadCast(new Net_ChatMessage($"{SessionVariables.instance.playerDictionary[playerId].playerName} can't join, lobby full! Ready players: ({players.Count}/{maxPlayers})"));
            }
            else
            {
                players.Add(playerId);
                SessionVariables.instance.server.BroadCast(new Net_ChatMessage($"{SessionVariables.instance.playerDictionary[playerId].playerName} is ready for the next round! Ready players: ({players.Count}/{maxPlayers})"));
            }
        }
    }

    // server only
    public void LeavePlayer(int playerId)
    {
        if (players.Contains(playerId))
        {
            players.Remove(playerId);
            int random = Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    SessionVariables.instance.server.BroadCast(new Net_ChatMessage($"{SessionVariables.instance.playerDictionary[playerId].playerName} chickend out! Ready players: ({players.Count}/{maxPlayers})"));
                    break;
                case 1:
                    SessionVariables.instance.server.BroadCast(new Net_ChatMessage($"{SessionVariables.instance.playerDictionary[playerId].playerName} didn't feel like losing! Ready players: ({players.Count}/{maxPlayers})"));
                    break;
                case 2:
                    SessionVariables.instance.server.BroadCast(new Net_ChatMessage($"{SessionVariables.instance.playerDictionary[playerId].playerName} is afraid of heights! Ready players: ({players.Count}/{maxPlayers})"));
                    break;
            }
        }

    }

    // client only
    private void OnTriggerEnter(Collider other)
    {
        if (inSession) return;
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            SessionVariables.instance.myGameClient.SendToServer(new Net_JoinGame(SessionVariables.instance.myPlayerId));
        }
    }

    // client only
    private void OnTriggerExit(Collider other)
    {
        if (inSession) return;
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            SessionVariables.instance.myGameClient.SendToServer(new Net_LeaveGame(SessionVariables.instance.myPlayerId));
        }
    }
}

[System.Serializable]
public class Score
{
    public int playerId;
    public float score;
    public bool finished;

    public Score(int playerId, float score)
    {
        this.playerId = playerId;
        this.score = score;
        finished = false;
    }

    public void AddScore(float time)
    {
        score += time;
    }
}

[System.Serializable]
public class Game
{
    public int game_id;
}