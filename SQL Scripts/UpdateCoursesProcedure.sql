SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      James Horrobin
-- Create Date: 10/09/2022
-- Description: Create or update course
-- =============================================
CREATE PROCEDURE UpdateCoursesProcedure
(
    @CourseId int,
	@StartDate datetime
)
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY
	    IF EXISTS (SELECT * FROM Courses WHERE CourseId = @CourseId)
			BEGIN
				UPDATE Courses
				SET StartDate = @StartDate
				WHERE CourseId = @CourseId;
			END
		ELSE
			BEGIN
				INSERT INTO Courses (CourseId, StartDate)
				VALUES (@CourseId, @StartDate);
			END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH
END
GO
