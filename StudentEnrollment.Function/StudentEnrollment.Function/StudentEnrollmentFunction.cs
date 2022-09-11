using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudentEnrollment.Function.Domain;
using StudentEnrollment.Function.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace StudentEnrollment.Function
{
    public class StudentEnrollmentFunction
    {
        private readonly IStudentEnrollmentService studentEnrollmentService;
        private readonly IValidator<StudentEnrollmentRequest> validator;

        public StudentEnrollmentFunction(IStudentEnrollmentService studentEnrollmentService, IValidator<StudentEnrollmentRequest> validator)
        {
            this.studentEnrollmentService = studentEnrollmentService;
            this.validator = validator;
        }

        [FunctionName("EnrollStudents")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Enrollment Request Recieved");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<List<StudentEnrollmentRequest>>(requestBody);

            var validationError = await ValidateRequests(data);

            if (string.IsNullOrEmpty(validationError))
            {
                var result = await this.studentEnrollmentService.EnrollStudentsAsync(data);

                return new OkObjectResult(result);
            }
            else
            {
                return new BadRequestErrorMessageResult(validationError);
            }
        }

        private async Task<string> ValidateRequests(List<StudentEnrollmentRequest> requests)
        {
            var errorsStringBuilder = new StringBuilder();

            foreach (var request in requests)
            {
                var result = await this.validator.ValidateAsync(request);
                
                result.Errors.ForEach((e) =>
                {
                    errorsStringBuilder.AppendLine(e.ErrorMessage);
                });
            }

            return errorsStringBuilder.ToString();
        }
    }
}
