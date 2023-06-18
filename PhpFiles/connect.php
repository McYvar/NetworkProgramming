<?php
$db_host = 'localhost';
$db_user = 'yvartoorop';
$db_pass = 'Ui5auquoDahr';
$db_name = 'yvartoorop';

/* Open a connection */
$mysqli = new mysqli("$db_host", "$db_user", "$db_pass", "$db_name");

/* check connection */
if ($mysqli->connect_errno) {
   echo "Failed to connect to MySQL: (" . $mysqli->connect_errno() . ") " . $mysqli->connect_error();
   exit();
}

/* error check */
function showerror($error, $errornr)
{
   die("Error (" . $errornr . ") " . $error);
}

function showjson($raport)
{
   if ($raport === 0) {
      echo "{\"results\":[{\"code\":0}]}";
   } else if ($raport === 1) {
      echo "{\"results\":[{\"code\":1}]}";
   } else {
      $raport["code"] = 1;
      echo "{\"results\":[" . json_encode($raport) . "]}";
   }
}

function showjsonserver($raport)
{
   if ($raport === 0) {
      echo "{\"servers\":[{\"code\":0}]}";
   } else if ($raport === 1) {
      echo "{\"servers\":[{\"code\":1}]}";
   } else {
      $raport["code"] = 1;
      echo "{\"servers\":[" . json_encode($raport) . "]}";
   }
}

function servercheck()
{
   if (is_null($_SESSION["server_id"])) {
      //echo "not assigned to a server<br>";
      return false;
   } else if ($_SESSION["server_id"] == -1) {
      //echo "server_id set to -1<br>";
      return false;
   } else {
      //echo "assigned to server<br>";
      return true;
   }
}

function usercheck()
{
   if (!is_null($_SESSION["user_id"])) {
      //echo "user logged in<br>";
      return true;
   } else {
      //echo "user not logged in<br>";
      return false;
   }
}
?>