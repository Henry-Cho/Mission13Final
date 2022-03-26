# Mission13Final

You would probably encounter an issue with deleting a record which is not created by you but are ones that the original database has.
Because the Bowler_Scores table has BowlerID as a foreign key which is the primary key of Bowlers table, you need to implement On Delete Cascade in order to make the delete function work.
So go to your BowlingLeagueStructureMy.SQL file and edit the content which starts on line 244 like this,

ALTER TABLE Bowler_Scores ADD 
CONSTRAINT Bowler_Scores_FK00 FOREIGN KEY (BowlerID) REFERENCES Bowlers (BowlerID) ON DELETE CASCADE,
ADD CONSTRAINT Bowler_Scores_FK01 FOREIGN KEY (MatchID, GameNumber) REFERENCES Match_Games (MatchID, GameNumber);

If you do this, your delete function will work fine
