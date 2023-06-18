<?php
include "connect.php";

$query = "SELECT * FROM games"; //query

if (!($result = $mysqli->query($query))) // query toepassen
  showerror($mysqli->errno,$mysqli->error); // als toepassen mislukt error laten zien

$my_json = "{\"users\":["; //aanmaken variabele die hele json gaat bevatten en beginvulling van json
$row = $result->fetch_assoc(); //haal eerste row uit "brei"

do { //begin van de loop om alle rows uit result te halen
  $my_json .= json_encode($row); //row omzetten naar json en toevoegen aan variabele
} while ($row = $result->fetch_assoc()); // einde loop

$my_json .= "]}"; // afsluiting json
echo $my_json; // json laten zien

?>