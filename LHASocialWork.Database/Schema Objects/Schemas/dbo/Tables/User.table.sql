CREATE TABLE [dbo].[Users] ( 
	UserId bigint identity(1,1) NOT NULL PRIMARY KEY,
	SystemUser bit NOT NULL DEFAULT 0,
	Email varchar(255) NOT NULL,
	[Password] varchar(255) NOT NULL,
	FirstName varchar(255) NOT NULL,
	LastName varchar(225) NOT NULL,
	PhoneNumber bigint NOT NULL,
	AddressId bigint NULL REFERENCES Addresses(AddressId),
	ProfilePictureId bigint NOT NULL REFERENCES Images(ImageId)
)