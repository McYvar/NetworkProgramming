<?php
include "connect.php";

session_start();

if (!usercheck()) {
    showjsonserver(0);
    die;
}

if (servercheck()) {
    // already on a server so log out of that one first
    $old_server_id = $_SESSION["server_id"];
    $query = "SELECT * FROM Servers WHERE id = '$old_server_id' LIMIT 1";
    if (!($result = $mysqli->query($query))) {
        showerror($mysqli->errno, $mysqli->error);
    }
    $row = $result->fetch_assoc();
    $fetched_user_amount = $row["connected_users"] - 1;

    $query = "UPDATE Servers SET connected_users = $fetched_user_amount WHERE id = $old_server_id";
    if (!($mysqli->query($query))) {
        showerror($mysqli->errno, $mysqli->error);
    }
}

// fetch results from url
$server_id = $_GET["server_id"];
$password = $_GET["password"];

// create and apply query
$query = "SELECT * FROM Servers WHERE id = '$server_id' LIMIT 1";
if (!($result = $mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}

// fetch array from result and fetch server_id from array
$row = $result->fetch_assoc();
$fetched_server_id = $row["id"];

// validate if server_id is 0 integer or is an integer at all
if (!(filter_var($server_id, FILTER_VALIDATE_INT) === 0)) {
    // validate if the server_id is an int
    if (filter_var($server_id, FILTER_VALIDATE_INT) === true) {
        //echo "server_id not an integer<br>";
        showjsonserver(0);
        die;
    }
}

// check if there are any results for the server number
if (is_null($fetched_server_id)) {
    //echo "no server found with $server_id!<br>";
    showjsonserver(0);
    die;
}

// check if the password changes after string replacement
$filtered_password = str_replace(" ", "", $password);
if (!($filtered_password === $password)) {
    //echo "invalid password<br>";
    showjsonserver(0);
    die;
}

// fetch password from array compare password with given password
$fetched_password = $row["password"];
if (!($fetched_password === hash('sha256', $password))) {
    //echo "incorrect password<br>";
    showjsonserver(0);
    die;
}

// passwords are a match, assign session variables
$_SESSION["session_id"] = session_id();
$_SESSION["server_id"] = $fetched_server_id;
$_SESSION["server_name"] = $row["server_name"];
$_SESSION["local"] = $row["local"];
$_SESSION["ip"] = $row["ip"];
$_SESSION["port"] = $row["port"];

// and increment the users in this server
$fetched_user_amount = $row["connected_users"] + 1;

$query = "UPDATE Servers SET connected_users = $fetched_user_amount WHERE id = $fetched_server_id";
if (!($mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}

// assign user to this server
$user_id = $_SESSION["user_id"];
$query = "UPDATE Users SET server_id = '$fetched_server_id' WHERE id = '$user_id' LIMIT 1";
if (!($mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}

showjsonserver($_SESSION);
?>