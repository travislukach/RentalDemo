CREATE TABLE [dbo].[Renter] (
    [RenterId]           INT          IDENTITY (1, 1) NOT NULL,
    [PropertyId]         INT          NOT NULL,
    [NumberOfOccupants]  INT          NOT NULL,
    [LastName]           VARCHAR (50) NOT NULL,
    [FirstName]          VARCHAR (50) NOT NULL,
    [PrimaryPhoneNumber] VARCHAR (50) NOT NULL,
    [StartDate]          DATETIME     NOT NULL,
    [EndDate]            DATETIME     NOT NULL,
    CONSTRAINT [PK_Renter] PRIMARY KEY CLUSTERED ([RenterId] ASC),
    CONSTRAINT [FK_Property_Renter] FOREIGN KEY ([PropertyId]) REFERENCES [dbo].[Property] ([PropertyId])
);

