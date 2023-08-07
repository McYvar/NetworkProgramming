<?php
include "connect.php";

// query to request all server_id's from database
$query = "SELECT id, ip, port, local, server_name, connected_users FROM Servers";
$result = execQuery($query);
$row = $result->fetch_assoc();
$did_first = false;
$to_json = "{\"servers\":[";
do {
    if ($did_first === false) {
        $did_first = true;
    } else {
        $to_json .= ",";
    }

    $row["server_id"] = $row["id"];
    unset($row["id"]);
    $to_json .= json_encode($row);
} while ($row = $result->fetch_assoc());
$to_json .= "]}";

echo $to_json;

?>