<?php
include "connect.php";

sessionCheck();

session_start();

if (!usercheck()) {
    showjsonserver(0);
    die;
}
if (!servercheck()) {
    showjsonserver(0);
    die;
}

$history_id = $_GET["history_id"];
$winner_id = $_GET["winner_id"];
$second_id = $_GET["second_id"];
$duration = $_GET["duration"];

$query = "UPDATE History SET winner_user_id = $winner_id, second_user_id = $second_id, game_duration = $duration WHERE id = $history_id LIMIT 1";
execQuery($query);

?>