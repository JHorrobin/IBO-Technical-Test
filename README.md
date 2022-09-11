# IBO-Technical-Test
IBO technical test submission.

## Build and Run

*Requires a SQL database*

1. Run Create Table Script.
2. Run Each Store Procedure Creation Script.
3. Run InsertExampleCourses Script.
3. Edit local.settings.json SQL connection to point to correct database.
4. Build project and either run locally or deploy to azure.

## Usage

Use postman make a post request to 

> [Root Address]/api/EnrollStudents

The body of the request should look something like this where the CourseCode correlates to the CourseId in the CourseDetails table.

```
[
    {
        "StudentId": 1234,
        "Forename": ,
        "Surname": "Horrobin",
        "Birthdate": "1995-09-27T00:00:00.000Z",
        "CourseCode": 1,
        "StudentContactDetails": {
            "Address": "Address",
            "Mobile": "Mobile",
            "Email": "Email",
            "Phone": "Phone",
            "PreferredContactMethod": 1
        }
    }
]
```

The response will look something like this 

```
{
    "success": true,
    "errorMessage": null,
    "result": [
        {
            "courseId": 1,
            "name": "Computer Science",
            "startDate": "2022-09-11T00:07:19.0090743Z",
            "students": [
                {
                    "studentId": 1234,
                    "forename": "James",
                    "surname": "Horrobin",
                    "birthDate": "1995-09-27T00:00:00Z",
                    "contactDetails": {
                        "address": "Address",
                        "mobile": "Mobile",
                        "email": "Email",
                        "phone": "Phone",
                        "preferredContactMethod": 1
                    }
                }
            ]
        }
    ]
}
```