<?php
include "connect.php";

$query = "SELECT * FROM games ORDER BY id DESC LIMIT 1";

if (!($result = $mysqli->query($query))) // query toepassen
  showerror($mysqli->errno, $mysqli->error); // als toepassen mislukt error laten zien

$my_json = "{\"users\":["; //aanmaken variabele die hele json gaat bevatten en beginvulling van json

$row = $result->fetch_assoc(); //info uit "brei" halen

echo json_encode($row); //json laten zien

?>