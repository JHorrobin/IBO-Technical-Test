using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Shouldly;
using StudentEnrollment.Function.Domain;
using StudentEnrollment.Function.Domain.Validation;
using StudentEnrollment.Function.Domain.Models;
using System.Text;
using System.Web.Http;

namespace StudentEnrollment.Function.Tests
{
    [TestClass]
    public class StudentEnrollmentFunctionTests
    {
        private readonly StudentEnrollmentFunction studentEnrollmentFunction;
        private readonly Mock<IStudentEnrollmentService> mockStudentEnrollmentService;
        private readonly IValidator<StudentEnrollmentRequest> validator;
        private readonly Mock<ILogger> mockLogger;

        public StudentEnrollmentFunctionTests()
        {
            this.mockLogger = new Mock<ILogger>();
            this.mockStudentEnrollmentService = new Mock<IStudentEnrollmentService>();
            this.validator = new StudentEnrollmentRequestValidator();
            this.studentEnrollmentFunction = new StudentEnrollmentFunction(this.mockStudentEnrollmentService.Object, this.validator);
        }

        [TestMethod]
        public async Task Run_Success_ReturnsActionResultWithSuccessObject()
        {
            var request = CreateMockRequest(new List<StudentEnrollmentRequest>());

            this.mockStudentEnrollmentService.Setup(s => s.EnrollStudentsAsync(It.IsAny<List<StudentEnrollmentRequest>>()))
                .ReturnsAsync(new StudentEnrollmentResult { });

            var result = await studentEnrollmentFunction.Run(request.Object, this.mockLogger.Object);

            var successResult = result as OkObjectResult;
            var successObject = successResult?.Value as StudentEnrollmentResult;
            successObject?.Success.ShouldBeTrue();
        }

        [TestMethod]
        public async Task Run_FailedValidator_ReturnsBadRequestWithMessage()
        {
            var request = CreateMockRequest(new List<StudentEnrollmentRequest>
            {
                new StudentEnrollmentRequest
                {
                    StudentId = 1234,
                    Forename = null,
                    Surname = "Surname",
                    Birthdate = DateTime.MinValue,
                    CourseCode = 1,
                    StudentContactDetails = new ContactDetails
                    {
                        PreferredContactMethod = PreferredContactMethod.None,
                        Phone = "1234",
                        Email = "Email",
                        Address = "Address",
                        Mobile = "1234"
                    }
                }
            });

            var result = await studentEnrollmentFunction.Run(request.Object, this.mockLogger.Object);
            var badRequestResult = result as BadRequestErrorMessageResult;
            badRequestResult?.Message.ShouldBe("'Forename' must not be empty.\r\n");
        }

        private Mock<HttpRequest> CreateMockRequest(object body)
        {
            var json = JsonConvert.SerializeObject(body);
            var byteArray = Encoding.ASCII.GetBytes(json);

            var memoryStream = new MemoryStream(byteArray);
            memoryStream.Flush();
            memoryStream.Position = 0;

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Body).Returns(memoryStream);

            return mockRequest;
        }
    }
}