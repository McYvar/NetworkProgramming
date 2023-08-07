<?php
include "connect.php";

sessionCheck();

session_start();
if (servercheck()) {
    showjson(0);
    die;
}
if (!usercheck()) {
    showjson(0);
    die;
}

// log user out and set server_id to -1
$user_id = $_SESSION["user_id"];

$query = "UPDATE Users SET server_id = -1 WHERE id = '$user_id'";
execQuery($query);

// clear and destroy session
session_unset();
session_destroy();

showjson(1);

?>