<?php
include "connect.php";

session_start();

if (usercheck()) {
    showjson(0);
    die;
}

$username = $_GET["username"];
$password = $_GET["password"];

// check if the username/email contains no spaces
$filtered_username = str_replace(" ", "", $username);
if (!($filtered_username === $username)) {
    //echo "invalid username/email<br>";
    showjson(0);
    die;
}

// check if the password contains no spaces
$filtered_password = str_replace(" ", "", $password);
if (!($filtered_password === $password)) {
    //echo "invalid password<br>";
    showjson(0);
    die;
}

// validate the if
$email = filter_var($username, FILTER_SANITIZE_EMAIL);

if (!filter_var($email, FILTER_VALIDATE_EMAIL) === false) {
    $query = "SELECT * FROM Users WHERE email = '$username' LIMIT 1";
} else {
    $query = "SELECT * FROM Users WHERE username = '$username' LIMIT 1";
}

if (!($result = $mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}

$row = $result->fetch_assoc();

// check if user exists
if (count($row) == 0) {
    //echo "user/email doesn't exist<br>";
    showjson(0);
    die;
}

// check if password is correct
if (!($row["password"] === hash('sha256', $password))) {
    //echo "incorrect password<br>";
    showjson(0);
    die;
}

$id = $row["id"];
$games_played = $row["games_played"];

// set session variables
$_SESSION["user_id"] = $id;
$_SESSION["email"] = $row["email"];
$_SESSION["username"] = $row["username"];

echo showjson($_SESSION);

?>