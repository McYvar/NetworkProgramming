<?php
include "connect.php";

session_start();

// log user out and set server_id to -1
$user_id = $_SESSION["user_id"];

$query = "UPDATE Users SET server_id = -1 WHERE id = '$user_id'";
execQuery($query);

// clear and destroy session
session_unset();
session_destroy();

showjson(1);

?>