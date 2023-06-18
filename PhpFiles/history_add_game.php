<?php
include "connect.php";

session_start();
/*
if (!usercheck()) {
    showjsonserver(0);
    die;
}
if (!servercheck()) {
    showjsonserver(0);
    die;
}
*/
$query = "INSERT INTO History(id, winner_user_id, second_user_id, game_duration) VALUES (null, null, null, null)";
if (!($result = $mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}

$query = "SELECT * FROM History ORDER BY id DESC LIMIT 1";
if (!($result = $mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}
$row = $result->fetch_assoc();

$_SESSION["game_id"] = $row["id"];

echo "{\"game_id\":\"" . $_SESSION["game_id"] . "\"}";

?>