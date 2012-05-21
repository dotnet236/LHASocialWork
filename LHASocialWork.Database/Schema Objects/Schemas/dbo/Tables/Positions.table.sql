CREATE TABLE [dbo].[Positions]
(
	PositionId bigint identity(1,1) NOT NULL PRIMARY KEY,
	SystemPosition bit NOT NULL Default 0,
	SystemRole varchar(255) NOT NULL,
	Name varchar(255) NOT NULL UNIQUE
)
