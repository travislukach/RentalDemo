-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE spSearchPropertiesByLocation
	@location VARCHAR(50)
AS
BEGIN
	SELECT * FROM Property WHERE Location LIKE '%'+@location+'%'
END