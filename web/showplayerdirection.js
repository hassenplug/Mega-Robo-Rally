function showplayerdirection(pl)
{
	var x = $("#playerdirection" + pl);
	x.load("programDirection.php?ShowID=" + pl);
}

