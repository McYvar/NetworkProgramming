<?php
include "connect.php";

session_start();

if (usercheck()) {
    showjson(0);
    die;
}

// first get the email, username and password
if (!isset($_GET["email"])) {
    //echo "email not set<br>";
    showjson(0);
    die;
}
$email = $_GET["email"];
if (empty($email)) {
    //echo "email was empty<br>";
    showjson(0);
    die;
}
// check if the email contains no spaces
$filtered_email = str_replace(" ", "", $email);
if (!($filtered_email === $email)) {
    //echo "invalid email<br>";
    showjson(0);
    die;
}

if (!isset($_GET["username"])) {
    //echo "username not set<br>";
    showjson(0);
    die;
}
$username = $_GET["username"];
if (empty($username)) {
    //echo "username was empty<br>";
    showjson(0);
    die;
}
// check if the username contains no spaces
$filtered_username = str_replace(" ", "", $username);
if (!($filtered_username === $username)) {
    //echo "invalid username<br>";
    showjson(0);
    die;
}


if (!isset($_GET["password"])) {
    //echo "password not set<br>";
    showjson(0);
    die;
}
$password = $_GET["password"];
if (empty($password)) {
    //echo "password not set<br>";
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

// validate email
$email = filter_var($email, FILTER_SANITIZE_EMAIL);
if (filter_var($email, FILTER_VALIDATE_EMAIL) === false) {
    //echo "$email is an invalid email<br>";
    showjson(0);
    die;
}

// then check if the username/email exists
$query = "SELECT * FROM Users WHERE (username = '$username' OR email = '$email')";
if (!($result = $mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}

$row = $result->fetch_assoc();

if (count($row) > 0) {
    if ($row["email"] === $email) {
        //echo "$email already exists<br>";
    }
    if ($row["username"] === $username) {
        //echo "$username already exists<br>";
    }
    showjson(0);
    die;
}

// hash the password
$hashed_password = hash('sha256', $password);

// now that we know no such user exists we can add them to the database
$query = "INSERT INTO Users(id, server_id, email, username, password, games_played) VALUES (NULL, -1, '$email' ,'$username', '$hashed_password', 0)";

if (!($mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}

// then retrieve the new user_id
$query = "SELECT * FROM Users WHERE username = '$username'";

if (!($result = $mysqli->query($query))) {
    showerror($mysqli->errno, $mysqli->error);
}

$row = $result->fetch_assoc();
$user_id = $row["id"];

// then set session variables
$_SESSION["user_id"] = $user_id;
$_SESSION["email"] = $email;
$_SESSION["username"] = $username;

showjson($_SESSION);

?>