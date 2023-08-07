<?php
include "connect.php";

// top win winners from last month
$query = "SELECT Users.id, Users.username, Scores.score
FROM ((History
       INNER JOIN Users ON History.winner_user_id = Users.id)
      INNER JOIN Scores ON History.id = Scores.history_id)
      WHERE History.game_date > CURRENT_DATE - STR_TO_DATE(\"00 01 0000\", \"%d %m %Y\")
      ORDER BY Scores.score LIMIT 5
";
$result1 = execQuery($query);
$table1 = execQuery($query);

// calc per player in the list how much games they played
$row = $result1->fetch_assoc();
do {
    $user_id = $row["id"];
    $query = "SELECT * FROM Scores WHERE user_id = '$user_id'";
    $count = execQuery($query);
    $winners[$user_id] = count($count->fetch_all());
} while ($row = $result1->fetch_assoc());

// top 5 seconds from last month
$query = "SELECT Users.id, Users.username, Scores.score
FROM ((History
       INNER JOIN Users ON History.second_user_id = Users.id)
      INNER JOIN Scores ON History.id = Scores.history_id)
      WHERE History.game_date > CURRENT_DATE - STR_TO_DATE(\"00 01 0000\", \"%d %m %Y\")
      ORDER BY Scores.score LIMIT 5
";
$result2 = execQuery($query);
$table2 = execQuery($query);

// calc per player in the list how much games they played
$row = $result2->fetch_assoc();
do {
    $user_id = $row["id"];
    $query = "SELECT * FROM Scores WHERE user_id = '$user_id'";
    $count = execQuery($query);
    $seconds[$user_id] = count($count->fetch_all());
} while ($row = $result2->fetch_assoc());

$did_first = false;
$row = $table1->fetch_assoc();
$json = "{\"top_5_first\":[";
do {
    if ($did_first === true) {
        $json .= ",";
    }
    $did_first = true;
    $json .= "{\"" . $row["username"] . "\":";
    $json .= "\"" . $row["score"] . "\"}";
} while ($row = $table1->fetch_assoc());
$json .= "]}";
echo $json;

$did_first = false;
$row = $table2->fetch_assoc();
$json = "<br>{\"top_5_second\":[";
do {
    if ($did_first === true) {
        $json .= ",";
    }
    $did_first = true;
    $json .= "{\"" . $row["username"] . "\":";
    $json .= "\"" . $row["score"] . "\"}";
} while ($row = $table2->fetch_assoc());
$json .= "]}";

echo $json;

?>