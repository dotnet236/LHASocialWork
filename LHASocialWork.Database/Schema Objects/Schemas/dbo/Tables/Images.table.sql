CREATE TABLE [dbo].[Images]
(
	ImageId bigint identity(1,1) NOT NULL PRIMARY KEY,
	Title varchar(255) NOT NULL,
	[Description] varchar(255) NULL,
	FileKey uniqueidentifier NOT NULL,
	OwnerId bigint REFERENCES Users(UserId),
	[Status] varchar(255)
)
