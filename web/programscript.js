function ClickAvailable(SourceCard,player){
	var CardID = document.getElementById('CardPriority'+SourceCard).textContent/10;
	
	if (CardID > 0)
	{
		var UseSlot = -1;
		var FullSlots = 0;
		for (var i = 5; i > -1; i--)
		{
			var CardPlayedSlot = document.getElementById('Priority'+i);
			if (CardPlayedSlot !==null)
			{
				var CardPlayedID = CardPlayedSlot.textContent/10;
				
				if (CardID === CardPlayedID)
				{
					// remove card already played
					ClickPlayed(i,player);
					return;
				}
				if (CardPlayedID > 0)
				{
					FullSlots++;
				}
				else
				{
					UseSlot = i;
				}
			}
		}
		if (UseSlot > -1) // found empty slot
		{
			var CardPlayedSlot = document.getElementById('Priority'+UseSlot);
			CardPlayedSlot.textContent = CardID * 10;
			
			var img = document.getElementById('Played' + UseSlot);
			img.src = document.getElementById('Available'+SourceCard).src;
			
			var bgc = document.getElementById('CardCell' + SourceCard);
			bgc.style.backgroundColor="ffffdd";
			// set card ID: CardID to played phase: i
				console.log('card:' + CardID + ' Played:' + (UseSlot+1) + ' Player:' + player);
			UpdateCardPlayed(CardID,UseSlot+1,player);
		}
		if (FullSlots>3)
		{
			// enable save
			var bgc1 = document.getElementById('CardCellA');
			bgc1.style.backgroundColor="88ff88";
			
		}
	}
}

function ClickPlayed(SourceCard,player){
	var CardPlayedSlot = document.getElementById('Priority'+SourceCard); // source priority
	if (CardPlayedSlot.textContent === "") // value of card
	{
	}
	else
	{
		var oldCard = CardPlayedSlot.textContent / 10;
		
		for (var i = 0; i <14; i++)
		{
			var CardPlayedSource = document.getElementById('CardPriority'+i);
			if (CardPlayedSource !== null)
			{
				var CardPlayedID = CardPlayedSource.textContent/10;
				if (CardPlayedID === oldCard)
				{
					var bgc = document.getElementById('CardCell' + i);
					bgc.style.backgroundColor="ddddff";
					
					// remove card from played list
					CardPlayedSlot.textContent = "";
					
					var img = document.getElementById('Played' + SourceCard);
					img.src = "ImageSetC/Blank.png";
					
					console.log('card:' + CardPlayedID + ' unplayed Player:' + player);
					UpdateCardPlayed(CardPlayedID,-1,player);
					// disable save
					var bgc2 = document.getElementById('CardCellA');
					bgc2.style.backgroundColor="f0f0f0";
					
				}
			}
		}
	}
}

function CallPage(pageURL)
{
	//console.log(pageURL);
	var xmlhttp = new XMLHttpRequest();
	
	xmlhttp.open("GET", pageURL , true);
	xmlhttp.send();
	//console.log("complete");
}

function UpdateCardPlayed(card,phase,player)
{
	var newsql = "setup.php?sqlstatement=CALL%20procUpdateCardPlayed(" + card + "," + phase + "," + player + ")";
	CallPage(newsql);
}

