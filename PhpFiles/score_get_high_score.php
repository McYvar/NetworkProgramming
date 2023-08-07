<?php
include "connect.php";

$query = "SELECT Users.username, Scores.score FROM Scores INNER JOIN Users ON Scores.user_id = Users.id ORDER BY score ASC";
$result = execQuery($query);
$row = $result->fetch_assoc();

$it = 0;
do {
    $it++;
    echo $it . ". " . $row["username"] . "; " . $row["score"] . "<br>";
} while ($row = $result->fetch_assoc());

?>