CREATE TABLE [dbo].[EventCoordinators]
(
	EventCoordinatorId bigint identity(1,1) NOT NULL PRIMARY KEY,
	EventId bigint NOT NULL REFERENCES Events(EventId),
	CoordinatorId bigint NULL REFERENCES Users(UserId),
	[Status] varchar(255) NOT NULL,
	LastStatusChange datetime NOT NULL,
	StartDate datetime NOT NULL,
	EndDate datetime NOT NULL
)
