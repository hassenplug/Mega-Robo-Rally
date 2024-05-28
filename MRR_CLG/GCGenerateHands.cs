// namespace MRR_CLG
// {
//     public partial class GameController 
//     {
//         public void GeneratePrograms()
//         {
//             //GenerateProgramsThread();
//             // start thread to call function
//             Thread CommandThread = new Thread(new ThreadStart(GenerateProgramsThread));
//             CommandThread.Name = "GenerateProgramsThread"; // +Name;
//             CommandThread.Start();
//             Thread.Sleep(1);
//         }

//         public void GenerateProgramsThread()
//         {
//             foreach (Player thisplayer in AllPlayers) //.Where(ap=>CardList)
//             {
//                 if ((thisplayer.PlayerStatus != tPlayerStatus.ReadyToRun) && (thisplayer.IsRunning))
//                 {
//                     GenerateProgramForPlayer(thisplayer);
//                 }
//             }

//             // clear list of commands...
//         }

//         public void GenerateProgramForPlayer(Player p_player)
//         {
//             if ((p_player.PlayerStatus == tPlayerStatus.ReadyToRun) || (!p_player.IsRunning))
//             {
//                 return;
//             }

//             int programmingPlayerID = p_player.ID;

//             //tPlayerStatus HoldStatus = p_player.PlayerStatus;
//             //p_player.PlayerStatus = tPlayerStatus.Programming;

//             //p_player.ComputerPlayer = true;

//             // 0 check distance to next flag
//             // 1 pick random set of cards
//             // 2 play out cards (shut down all other robots)
//             // 3 check result distance to next flag, again
//             // 3a dead = bad
//             // 3b distance to flag
//             // 3c damage collected
//             // 4 repeat #1 for all possible hands

//             // 0 check distance to next flag
//             //int DistanceToGoal = p_player.DistanceToNextFlag;
//             int BestScore = -2;
//             CardList BestCardList = new CardList(); // p_player.PlayedCards;
//             tShutDown BestShutDown = tShutDown.None;
//             bool continueChecking = true;
//             bool lived = false;

//             int randomhandcount = 0;

//             do
//             {
//                 //Console.WriteLine("Generate program for player " + p_player.ID.ToString() + " remaining attempts:" + randomhandcount.ToString()); ;
//                 randomhandcount++;
//                 continueChecking = (randomhandcount < 10);

//                 //CardList UseCardList = p_player.PlayerCards;

//                 // 1 pick random set of cards

//                 // clear commands
//                 ListOfCommands.Clear(); // = new CommandList();
//                 p_player = LoadOneRobot(programmingPlayerID);

//                 // create working players here...
//                 // (shut down all other robots)
//                 //AllPlayers = new Players(AllPlayers);
//                 //Player WatchingPlayer = p_player;

//                 GiveThisPlayerARandomHand(p_player);

//                 // 2 play out cards
//                 // begin moves
//                 for (int RunningPhase = 1; RunningPhase < PhaseCount + 1; RunningPhase++)
//                 {
//                     ExecutePhase(RunningPhase, false);
//                 }

//                 // 3 check score
//                 int newScore = p_player.PlayerScore;

//                 if (p_player.IsDead)
//                 {
//                     //newScore = -1;
//                     if (!lived) continueChecking = true;
//                 }
//                 else
//                 {
//                     lived = true;
//                 }


//                 // 4 compare and save
//                 if (newScore > BestScore)
//                 {
//                     BestScore = newScore;
//                     //BestCardList = WatchingPlayer.PlayedCards ;
//                     BestCardList.Clear();
//                     foreach (MoveCard thiscard in p_player.CardsPlayed.OrderBy(pc => pc.PhasePlayed))
//                     {
//                         MoveCard newcard = new MoveCard(thiscard.ID);
//                         newcard.PhasePlayed = thiscard.PhasePlayed;
//                         if (!thiscard.Locked) BestCardList.Add(newcard);
//                     }

//                     if (p_player.Damage > 4) BestShutDown = tShutDown.NextTurn;
//                     else BestShutDown = tShutDown.None;

//                 }

//                 //ClearCardsForPlayer(p_player, 0);
//                 if (randomhandcount > 50) continueChecking = false;

//             } while (continueChecking);
//             // 5 repeat #1 for all possible hands

//             //ClearCardsForPlayer(p_player, 0);

//             // program this hand
//             foreach (MoveCard thiscard in BestCardList)
//             {
//                 MoveCard realcard = GameCards.First(gc => gc.ID == thiscard.ID);
//                 realcard.Owner = p_player.ID;
//                 //PlayCard(realcard);
//             }

//             p_player.ShutDown = BestShutDown;

//             //Console.WriteLine("Program for player " + p_player.ID.ToString() + " has " + p_player.PlayedCards.Count().ToString() + " cards and ends " + BestScore.ToString() + " from goal, with " + randomhandcount.ToString() + " attempts");

//             //p_player.PlayerStatus = HoldStatus;
//             //Thread.Sleep(1);
//         }
        
// }
// }