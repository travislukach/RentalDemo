CREATE TABLE [dbo].[Property] (
    [PropertyId]          INT            IDENTITY (1, 1) NOT NULL,
    [PropertyName]        VARCHAR (50)   NOT NULL,
    [DailyRate]           MONEY          NOT NULL,
    [MaximumOccupants]    INT            NOT NULL,
    [Location]            VARCHAR (50)   NOT NULL,
    [NonStandardFeatures] VARCHAR (2000) NULL,
    CONSTRAINT [PK_Property] PRIMARY KEY CLUSTERED ([PropertyId] ASC)
);