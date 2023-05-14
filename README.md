# CookieCutter.UI.Tests
Cookie Cutter UI Tests

This repository contains UI test framework created using:
 - Specflow BDD
 - .net 7.0
 - Selenium 4.9.1
 - Specflow Nunit 3.9.74
 - FluentAssertions 6.11.0


# Pre-requisites
 - Visual Studio 2022 Community edition or Visual studio code
 - .net SDK 7.0 needs to be installed on the machine (https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-7.0.203-macos-x64-installer)
 - Install SpecFlow extension and restart Visual Studio (This is not available if using a mac)
 - If test explorer is not visible, Goto Test menu and enable Test Explorer 
 - If specflow tests are run for the first time and the tests do not execute then Specflow code needs activation, this code is generated in the log files (log files are auto generated in the Test Results Directory
 - You will need to register with this code on Specflow's official website and name and a valid email Address would be asked to activate specflow for free

 # Building the code 

1. Clone the repo
2. Restore the nuget packages 

Open a terminal and run

```
docker run --rm -it -p 4444:4444 -p 7900:7900 --shm-size 2g selenium/standalone-chrome:dev (Leave the terminal open)
```

Open another terminal and run

```
cd Cookie.Ui.Tests
dotnet restore
```
3. Build the project
```
dotnet build 
```
4. Run the tests
```
dotnet test --logger "html;LogFileName=TestResults.html"
```

# Running Tests
 - Once all pre-requisites are satisfied and the build generation is succesful, the tests should be visible in the test explorer.
 - Click Run on all tests or select individual test to run

# Note:
 - If the config does not seem to find External Connections (when running locally without docker), this is because the appsettings.test.json file is not auto copied to Test Results folder, I am not able to solve this issue, Please can you copy this file manually to test results folder for the tests to start running (I tried everything but I am not able to sort this issue for both the repos, Setting property to Copying always does not solve the issue). Below is the error that needs the appsettings.test.json file to be manually copied to TestResults folder
** -   Message: 
    Object reference not set to an instance of an object.
  Stack Trace: 
    System.NullReferenceException: Object reference not set to an instance of an object.
    ConfigurationGenerator.SetInitialConfiguration(ConfigurationHelper configurationHelper) **


# Running tests using docker (Currently configured only for Chrome)

Open a terminal and run the following commands (Leave terminal open)
```
docker run --rm -it -p 4444:4444 -p 7900:7900 --shm-size 2g selenium/standalone-chrome:dev
```


Open another terminal and run the below commands
```
cd Cookie.Ui.Tests
docker build --no-cache -f "CookieClicker.dockerfile" -t cc-tests .
docker run --network host -e WebDriver=remote -e Test_Browser=chrome cc-tests
```

# List of Identified Bugs:

1. The initial Money value displayed on the game page shows as $0.0, but then it updates immediately to display $0.
2. Copying the cookie count from the page is not possible due to continuous updates caused by the factories.
3. Clicking to add 1 cookie and then entering 1 in the "Sell Cookies!" input field does not affect the money or cookie count. If selling is not possible with only 1 cookie, then the expected behaviour is that the "Sell Cookies!" button should be greyed out.
4. When there are 3 cookies and 1.5 cookies are sold, the count changes to 2 instead of 1.5.
5. When there are 10 cookies available and I try to sell 30 cookies, the sell order does not go through until the cookie count is greater than 30, there is no message displayed to the user stating that they cannot sell.