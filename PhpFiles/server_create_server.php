<?php
include "connect.php";

session_start();

if (!usercheck()) {
    showjsonserver(0);
    die;
}

// get from url: ip, local, server_name, password and check input and validate
if (!isset($_GET["ip"])) {
    showjsonserver(0);
    die;
}
$ip = $_GET["ip"];
if (empty($ip) || filter_var($ip, FILTER_VALIDATE_IP) === false) {
    showjsonserver(0);
    die;
}

if (!isset($_GET["port"])) {
    showjsonserver(0);
    die;
}
$port = $_GET["port"];
if (filter_var($local, FILTER_VALIDATE_INT) === 0 || !filter_var($port, FILTER_VALIDATE_INT) === false) {
    // do nothing, is valid
} else {
    showjsonserver(0);
    die;
}

if (!isset($_GET["local"])) {
    showjsonserver(0);
    die;
}
$local = $_GET["local"];
if (filter_var($local, FILTER_VALIDATE_INT) === 0 || !filter_var($local, FILTER_VALIDATE_INT) === false) {
    // do nothing, is valid
} else {
    showjsonserver(0);
    die;
}

if (!isset($_GET["server_name"])) {
    showjsonserver(0);
    die;
}
$server_name = $_GET["server_name"];
if (empty($server_name)) {
    showjsonserver(0);
    die;
}
$filtered_serever_name = str_replace(" ", "", $server_name);
if (!($filtered_serever_name === $server_name)) {
    showjsonserver(0);
    die;
}
// is server name is unique by checking if it's available
$query = "SELECT * FROM Servers WHERE server_name = '$server_name' LIMIT 1";
if (!($result = $mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}
$row = $result->fetch_assoc();
if (count($row) > 0) {
    showjsonserver(0);
    die;
}

if (!isset($_GET["password"])) {
    showjsonserver(0);
    die;
}
if (empty($_GET["password"])) {
    showjsonserver(0);
    die;
}
$password = hash('sha256', $_GET["password"]);
// check if the password contains no spaces
$filtered_password = str_replace(" ", "", $password);
if (!($filtered_password === $password)) {
    //echo "invalid password<br>";
    showjsonserver(0);
    die;
}

// now add a server to db with details and auto login
$query = "INSERT INTO Servers(id, ip, port, local, server_name, password, connected_users) VALUES (NULL, '$ip', '$port', '$local', '$server_name', '$password', 1)";
if (!($mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}

// fetch the server id with another query
$query = "SELECT id FROM Servers WHERE server_name = '$server_name'";
if (!($result = $mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}
$row = $result->fetch_assoc();

// and set session variables
$server_id = $row["id"];
$_SESSION["session_id"] = session_id();
$_SESSION["server_id"] = $server_id;
$_SESSION["server_name"] = $server_name;
$_SESSION["local"] = $local;
$_SESSION["ip"] = $ip;
$_SESSION["port"] = $port;

// assign user to this server
$user_id = $_SESSION["user_id"];
$query = "UPDATE Users SET server_id = '$server_id' WHERE id = '$user_id' LIMIT 1";
if (!($mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}

showjsonserver($_SESSION);
?>