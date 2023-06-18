<?php
include "connect.php";

session_start();
if (!usercheck()) {
    showjson(0);
    die;
}

$user_id = $_SESSION["user_id"];
// check for confirmation password
if (!isset($_GET["password"]) || empty($_GET["password"])) {
    //echo "password not set";
} else {
    // password was set so compare
    //$password = $_GET["password"];
    $password = hash('sha256', $_GET["password"]);
    $query = "SELECT password FROM Users WHERE id = '$user_id' LIMIT 1";

    if (!($result = $mysqli->query($query))) {
        showerror($mysqli->errno, $mysqli->error);
    }

    $row = $result->fetch_assoc();

    // check if password is correct
    if (!($row["password"] === $password)) {
        //echo "incorrect password<br>";
        showjson(0);
        die;
    }
}

$can_update = true;
$reason = "";

// Update only relevant data, only email, username and password can be changed
// check for new email input
if (isset($_GET["email"]) && !empty($_GET["email"])) {
    $email = $_GET["email"];

    // check if email exists in database
    $query = "SELECT email FROM Users WHERE email = '$email' LIMIT 1";
    if (!($result = $mysqli->query($query))) {
        showerror($mysqli->errno, $mysqli->error);
    }
    $row = $result->fetch_assoc();
    if (count($row) > 0) {
        $can_update = false;
        $reason .= "email already exists<br>";
    }
    // check if the email contains no spaces
    $filtered_email = str_replace(" ", "", $email);
    if (!($filtered_email === $email)) {
        $can_update = false;
        $reason .= "invalid email<br>";
    }
    // validate email
    $email = filter_var($email, FILTER_SANITIZE_EMAIL);
    if (filter_var($email, FILTER_VALIDATE_EMAIL) === false) {
        $can_update = false;
        $reason .= "$email is not a valid email<br>";
    }
}

// check for new username input
if (isset($_GET["username"]) && !empty($_GET["username"])) {
    $username = $_GET["username"];

    // check if username exists in database
    $query = "SELECT username FROM Users WHERE username = '$username' LIMIT 1";
    if (!($result = $mysqli->query($query))) {
        showerror($mysqli->errno, $mysqli->error);
    }
    $row = $result->fetch_assoc();
    if (count($row) > 0) {
        $can_update = false;
        $reason .= "username already exists<br>";
    }
    // check if the username contains no spaces
    $filtered_username = str_replace(" ", "", $username);
    if (!($filtered_username === $username)) {
        $can_update = false;
        $reason .= "invalid username<br>";
    }
}

// check for new password input
if (isset($_GET["new_password"]) && !empty($_GET["new_password"])) {
    $new_password = $_GET["new_password"];

    // check if the password contains no spaces
    $filtered_password = str_replace(" ", "", $new_password);
    if (!($filtered_password === $new_password)) {
        $can_update = false;
        $reason .= "invalid password<br>";
    }

    $hashed_password = hash('sha256', $new_password);
}

// if something went wrong...
if ($can_update === false) {
    //echo $reason;
    showjson(0);
    die;
}

// update database and session variables
if (!empty($email)) {
    $query = "UPDATE Users SET email = '$email' WHERE id = '$user_id'";
    if (!($mysqli->query($query))) {
        showerror($mysqli->errno, $mysqli->error);
    }
    $_SESSION["email"] = $email;
}
if (!empty($username)) {
    $query = "UPDATE Users SET username = '$username' WHERE id = '$user_id'";
    if (!($mysqli->query($query))) {
        showerror($mysqli->errno, $mysqli->error);
    }
    $_SESSION["username"] = $username;
}
if (!empty($new_password)) {
    $query = "UPDATE Users SET password = '$hashed_password' WHERE id = '$user_id'";
    if (!($mysqli->query($query))) {
        showerror($mysqli->errno, $mysqli->error);
    }
}

showjson(1);
?>