CREATE TABLE [dbo].[Events]
(
	EventId bigint identity(1,1) NOT NULL PRIMARY KEY,
	Name varchar(255) not null UNIQUE,
	[Description] TEXT NOT NULL,
	LocationId bigint NOT NULL REFERENCES Addresses(AddressId),
	OwnerId bigint NOT NULL REFERENCES Users(UserId),
	FlyerId bigint NOT NULL REFERENCES Images(ImageId),
	StartDate datetime NOT NULL,
	StartTime datetime NOT NULL,
	EndDate datetime NOT NULL,
	EndTime datetime NOT NULL,
	EventType varchar(255) NOT NULL DEFAULT('Event'),
	Occurrence varchar(255) not null,
	Privacy varchar(255) not null
)
