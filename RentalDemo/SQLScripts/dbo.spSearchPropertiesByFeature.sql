-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spSearchPropertiesByFeature]
	@feature VARCHAR(50)
AS
BEGIN
	SELECT * FROM Property WHERE NonStandardFeatures LIKE '%'+@feature+'%'
END