CREATE TABLE [dbo].[EventMembers]
(
	EventMemberId bigint identity(1,1) NOT NULL PRIMARY KEY,
	EventId bigint NOT NULL REFERENCES Events(EventId),
	UserId bigint NULL REFERENCES Users(UserId),
	EmailAddress varchar(255) NULL,
	FacebookUser bigint NULL,
	[Status] varchar(255) NOT NULL,
	LastStatusChange datetime NULL
)
