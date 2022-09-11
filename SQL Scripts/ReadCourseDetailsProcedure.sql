SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      James Horrobin
-- Create Date: 10/09/2020
-- Description: Read Course Details
-- =============================================
CREATE PROCEDURE ReadCourseDetailsProcedure
AS
BEGIN
    SELECT [CourseId], [CourseName] FROM [CourseDetails]
END
GO
