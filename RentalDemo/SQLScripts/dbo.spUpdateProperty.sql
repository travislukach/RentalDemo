-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateProperty] 
	-- Add the parameters for the stored procedure here
	@id INT,
	@name VARCHAR(50) = '',
	@rate MONEY = 0.00,
	@maxOccupants INT = 0,
	@location VARCHAR(50) = '',
	@nonStandardFeatures VARCHAR(2000) = ''

AS
BEGIN
	DECLARE @defaultString VARCHAR(50) = ''
	DECLARE @defaultMoney MONEY = 0.00
	DECLARE @defaultInt INT = 0
	DECLARE @dynamicSQL NVARCHAR(4000) = 'UPDATE Property SET '
	DECLARE @dynamicSQLFinish VARCHAR(50) = ' WHERE PropertyId = '+ CONVERT(varchar(8),@id);
	
	DECLARE @updateFlag BIT = 0
	
	DECLARE @currentName VARCHAR(50) = (SELECT [PropertyName] FROM Property WHERE PropertyId = @id)
	DECLARE @currentRate MONEY = (SELECT DailyRate FROM Property WHERE PropertyId = @id)
	DECLARE @currentMaxOcc INT = (SELECT MaximumOccupants FROM Property WHERE PropertyId = @id)
	DECLARE @currentLocation VARCHAR(50) = (SELECT Location FROM Property WHERE PropertyId = @id)
	DECLARE @currentFeatures VARCHAR(2000) = (SELECT NonStandardFeatures FROM Property WHERE PropertyId = @id)

	IF @name <> @defaultString AND @name <> @currentName
	BEGIN
		SET @updateFlag = 1
		SET @dynamicSQL = @dynamicSQL + '[PropertyName] = '''+@name+''''
	END

	IF @rate <> @defaultMoney AND @rate <> @currentRate
	BEGIN
		IF @updateFlag = 1
		BEGIN
			SET @dynamicSQL = @dynamicSQL + ', '
		END
		ELSE
		BEGIN
			SET @updateFlag = 1
		END
		SET @dynamicSQL = @dynamicSQL + '[DailyRate] = '+CONVERT(nvarchar(10),@rate)
	END

	IF @maxOccupants <> @defaultInt AND @maxOccupants <> @currentMaxOcc
	BEGIN
		IF @updateFlag = 1
		BEGIN
			SET @dynamicSQL = @dynamicSQL + ', '
		END
		ELSE
		BEGIN
			SET @updateFlag = 1
		END
		SET @dynamicSQL = @dynamicSQL + '[MaximumOccupants] = '+ CONVERT(nvarchar(10),@maxOccupants)
	END

	IF @location <> @defaultString AND @location <> @currentLocation
	BEGIN
		IF @updateFlag = 1
		BEGIN
			SET @dynamicSQL = @dynamicSQL + ', '
		END
		ELSE
		BEGIN
			SET @updateFlag = 1
		END
		SET @dynamicSQL = @dynamicSQL + '[Location] = '''+@location+''''
	END

	IF @nonStandardFeatures <> @defaultString AND @nonStandardFeatures <> @currentFeatures
	BEGIN
		IF @updateFlag = 1
		BEGIN
			SET @dynamicSQL = @dynamicSQL + ', '
		END
		ELSE
		BEGIN
			SET @updateFlag = 1
		END
		SET @dynamicSQL = @dynamicSQL + '[NonStandardFeatures] = '''+@nonStandardFeatures+''''
	END

	SET @dynamicSQL = @dynamicSQL +''+ @dynamicSQLFinish

	BEGIN TRY
		EXEC (@dynamicSQL)
	END TRY
	BEGIN CATCH
	SELECT   
        ERROR_NUMBER() AS ErrorNumber,  
        ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END