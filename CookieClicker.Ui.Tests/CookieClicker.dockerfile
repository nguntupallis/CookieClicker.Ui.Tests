# Stage 1: Build the project
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY . .

# Set the environment variable to "remote"
ENV WebDriver=remote

# RUN dotnet restore
RUN dotnet build --configuration Release

# Copy the appsettings.test.json file to the /app/bin/Debug/net7.0/ directory
COPY appsettings.test.json /app/bin/Debug/net7.0/
COPY appsettings.test.json /app/bin/Release/net7.0/

# Stage 2: Run tests
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS test
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "test"]
# CMD ["dotnet", "test", "--logger", "html;LogFileName=/app/TestResults/TestResults.html", "--no-build"]