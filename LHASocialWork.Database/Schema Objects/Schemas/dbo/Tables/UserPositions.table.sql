CREATE TABLE [dbo].[UserPositions]
(	
	UserPositionId bigint identity(1,1) NOT NULL PRIMARY KEY,
	UserId bigint NOT NULL REFERENCES Users(UserId),
	PositionId bigint NOT NULL REFERENCES Positions(PositionId)
)
