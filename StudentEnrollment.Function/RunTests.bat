dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
reportgenerator -reports:"StudentEnrollment.Function.Tests\coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:Html