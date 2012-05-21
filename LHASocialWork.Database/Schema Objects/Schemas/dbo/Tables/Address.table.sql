CREATE TABLE [dbo].[Addresses]
(
	AddressId bigint IDENTITY (1,1) NOT NULL PRIMARY KEY,
	Street varchar(255) NOT NULL,
	City varchar(255) NOT NULL,
	Province varchar(255) NULL,
	[State] varchar(255) NULL,
	Country varchar(255) NOT NULL,
	Zip varchar(255) NOT NULL
)
