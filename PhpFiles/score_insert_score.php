<?php
include "connect.php";

sessionCheck();

session_start();
if (!servercheck()) {
    showjson(0);
    die;
}
if (!usercheck()) {
    showjson(0);
    die;
}

// first we get the score, user_id and history_id
$score = $_GET["score"];
$user_id = $_GET["user_id"];
$history_id = $_GET["history_id"];

// then we check if the history id is valid, can't save scores if no game was ended first
$query = "SELECT id FROM History WHERE id = '$history_id'";
$row = execQuery($query)->fetch_assoc();

// check if game exists
if (count($row) == 0) {
    //echo "Game with id$history_id doesn't exist!";
    showjson(0);
    die;
}

// game exists so add score
$query = "INSERT INTO Scores(id, history_id, user_id, score) VALUES (NULL, '$history_id', '$user_id', '$score')";
execQuery($query);

// add a game amount to the user_id
$query = "SELECT games_played FROM Users WHERE id = '$user_id' LIMIT 1";
$row = execQuery($query)->fetch_assoc();
$amount = $row["games_played"] + 1;

$query = "UPDATE Users SET games_played = '$amount' WHERE id = '$user_id' LIMIT 1";
execQuery($query);

showjson(1);

?>