<?php
include "connect.php";

sessionCheck();

// get server_id from url
if (!isset($_GET["server_id"])) {
    showjson(0);
    die;
}
$server_id = $_GET["server_id"];
if (empty($server_id)) {
    showjson(0);
    die;
}

// check if server exists in database
$query = "SELECT id FROM Servers WHERE id = '$server_id'";
$row = execQuery($query)->fetch_assoc();
if (!count($row) > 0) {
    showjson(0);
    die;
}

// check database for all players in this server
$query = "SELECT id, username FROM Users WHERE server_id = '$server_id'";
$result = execQuery($query);

// loop trough result to register all id's
$my_json = "{\"results\":[";
$row = $result->fetch_assoc();

$did_first = false;
do {
    if ($did_first === false) {
        $did_first = true;
    } else {
        $my_json .= ",";
    }

    $row["user_id"] = $row["id"];
    unset($row["id"]);
    $my_json .= json_encode($row);
} while ($row = $result->fetch_assoc());

$my_json .= "]}";
echo $my_json;
?>