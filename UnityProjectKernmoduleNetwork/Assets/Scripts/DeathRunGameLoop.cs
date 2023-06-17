using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DeathRunGameLoop : MonoBehaviour
{
    // first we have 2/4 players
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
    private List<int> sessionPlayers = new List<int>();

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

    private void RequestGameStart()
    {
        if (players.Count < 1) return;
        SessionVariables.instance.myGameClient.SendToServer(new Net_StartGame());
    }

    public void StartGame()
    {
        if (inSession) return;
        gameTime = Time.time;
        inSession = true;
        playersWhoNotPlayedDeathThisSession.Clear();
        playerScore.Clear();
        foreach (int player in players)
        {
            playersWhoNotPlayedDeathThisSession.Add(player);
            playerScore.Add(player, new Score(player, 0));
            sessionPlayers.Add(player);
        }
        playersReachedGoal = 0;
        StartCoroutine(webRequest.Request<Game>("https://studenthome.hku.nl/~yvar.toorop/php/history_add_game", (request) =>
        {
            if (request != null)
            {
                gameId = request.game_id;
                NextPlayer();
            }
        }));
    }

    private int FindNextPlayer()
    {
        int random = UnityEngine.Random.Range(0, playersWhoNotPlayedDeathThisSession.Count);
        int result = playersWhoNotPlayedDeathThisSession[random];
        playersWhoNotPlayedDeathThisSession.RemoveAt(random);
        return result;
    }

    public void NextPlayer()
    {
        if (SessionVariables.instance.server == null) return;

        if (playersWhoNotPlayedDeathThisSession.Count == 0)
        {
            EndGame();
            return;
        }

        int deathPlayer = FindNextPlayer();
        currentDeath = deathPlayer;
        foreach (var player in sessionPlayers)
        {
            if (player == deathPlayer) continue;
            SessionVariables.instance.server.BroadCast(new Net_TeleportPlayer(player, runnersSpawn.position.x, runnersSpawn.position.y, runnersSpawn.position.z));
        }

        SessionVariables.instance.server.BroadCast(new Net_TeleportPlayer(deathPlayer, deathSpawn.position.x, deathSpawn.position.y, deathSpawn.position.z));
        SessionVariables.instance.server.BroadCast(new Net_StartRound());
    }

    public void StartRound()
    {
        foreach (var player in playerScore.Values) player.finished = false;
        roundTime = Time.time;
        if (SessionVariables.instance.server != null) SessionVariables.instance.server.BroadCast(new Net_OpenBarriers());
    }

    public void OpenBarriers()
    {
        foreach (Barriers barrier in allBarriers)
        {
            barrier.OpenBarriers();
        }
    }

    public void EndRound()
    {
        SessionVariables.instance.server.BroadCast(new Net_CloseBarriers());
        StartCoroutine(WaitForNextTurn(5));
    }

    public void CloseBarriers()
    {
        foreach (Barriers barrier in allBarriers)
        {
            barrier.CloseBarriers();
        }
    }

    public void EndGame()
    {
        if (SessionVariables.instance.server == null) return;
        gameTime = Time.time - gameTime;
        inSession = false;
        foreach (var player in sessionPlayers)
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

        Score firstPlace = playerScore[sessionPlayers[0]];
        foreach (var score in playerScore.Values)
        {
            if (score.score < firstPlace.score)
            {
                firstPlace = score;
            }
        }

        Score secondPlace = playerScore[sessionPlayers[0]];
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

    public void ReachedGoal(int playerId)
    {
        if (SessionVariables.instance.server != null)
        {
            if (!sessionPlayers.Contains(playerId)) return;
            if (!playerScore[playerId].finished)
            {
                playerScore[playerId].finished = true;
                playersReachedGoal++;
                if (playersReachedGoal == 2) SessionVariables.instance.server.BroadCast(new Net_ChatMessage($"{SessionVariables.instance.playerDictionary[playerId].playerName} reached the finish 2nd place!"));
                else SessionVariables.instance.server.BroadCast(new Net_ChatMessage($"{SessionVariables.instance.playerDictionary[playerId].playerName} reached the finish {playersReachedGoal}th place!"));
                if (playerId != currentDeath) playerScore[playerId].AddScore(Time.time - roundTime);
                if (playersReachedGoal >= sessionPlayers.Count)
                {
                    EndRound();
                }
            }
        }
    }

    private IEnumerator WaitForNextTurn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        NextPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (players.Count >= maxPlayers) return;
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            foreach (var player in SessionVariables.instance.playerDictionary.Values)
            {
                Debug.Log($"{player.playerObject.name} . {other.gameObject.name}");
                if (player.playerObject == other.gameObject)
                {
                    if (!players.Contains(player.playerId)) players.Add(player.playerId);
                    return;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            foreach (var player in SessionVariables.instance.playerDictionary.Values)
            {
                if (player.playerObject == other.gameObject)
                {
                    if (players.Contains(player.playerId)) players.Remove(player.playerId);
                    return;
                }
            }
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