<?php
include "connect.php";

session_start();

if (!servercheck()) {
    showjsonserver(0);
    die;
}
if (!usercheck()) {
    showjsonserver(0);
    die;
}

$server_id = $_SESSION["server_id"];

// retrieve current connected_users
$query = "SELECT connected_users FROM Servers WHERE id = '$server_id' LIMIT 1";

if (!($result = $mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}

$row = $result->fetch_assoc();
$connected_users = $row["connected_users"] - 1;

if ($connected_users == 0) {
    // server is empty and can be deleted
    $query = "DELETE FROM Servers WHERE id = '$server_id' LIMIT 1";
} else {
    // remove user from active server
    $query = "UPDATE Servers SET connected_users = '$connected_users' WHERE id = '$server_id'";
}
if (!($mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}

// set server from user to -1
$user_id = $_SESSION["user_id"];
$query = "UPDATE Users SET server_id = -1 WHERE id = '$user_id'";
if (!($mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}

unset($_SESSION["session_id"]);
unset($_SESSION["server_id"]);
unset($_SESSION["server_name"]);
unset($_SESSION["local"]);
unset($_SESSION["ip"]);
unset($_SESSION["port"]);

showjsonserver(1);

?>