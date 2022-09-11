SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      James Horrobin
-- Create Date: 10/09/2022
-- Description: Create or update student
-- =============================================
CREATE PROCEDURE UpdateStudentProcedure
(
	@StudentId int,
    @ForeName varchar(200),
    @Surname varchar(200),
	@BirthDate datetime,
	@CourseId int,
	@Address varchar(200),
	@Mobile varchar(20),
	@Email varchar(200),
	@Phone varchar(20),
	@PreferredContactMethod int
)
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY
		IF EXISTS (SELECT * FROM Students WHERE StudentId = @StudentId)
			BEGIN
				UPDATE Students
				SET Forname = @ForeName, Surname = @Surname, BirthDate = @BirthDate, CourseId = @CourseId
				WHERE StudentId = @StudentId
			END
		ELSE
			BEGIN
				INSERT INTO Students (StudentId, Forname, Surname, Birthdate, CourseId)
				VALUES (@StudentId, @ForeName, @Surname, @BirthDate, @CourseId);
			END

		IF EXISTS (SELECT * FROM ContactDetails WHERE StudentId = @StudentId)
			BEGIN
				UPDATE ContactDetails
				SET [Address] = @Address, Mobile = @Mobile, Email = @Email, Phone = @Phone, PreferredContactMethod = @PreferredContactMethod
				WHERE StudentId = @StudentId
			END
		ELSE
			BEGIN
				INSERT INTO ContactDetails(StudentId, [Address], Mobile, Email, Phone, PreferredContactMethod)
				VALUES (@StudentId, @Address, @Mobile, @Email, @Phone, @PreferredContactMethod);
			END

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH
END
GO
