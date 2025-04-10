# MikrocopUsers

Test assignment

To run:
Download and install docker desktop - https://www.docker.com/products/docker-desktop/

Clone project and run docker compose up --build  with CLI in project folder.

![image](https://github.com/user-attachments/assets/c07ee520-878a-4743-816c-32de66c59bb1)

You need to have a https certificate for this to work, otherwise api container will not start.
Easiest way to get this is to run **dotnet dev-certs https --export-path ./certs/webapicert.pfx --password "addpassword"** in the root folder of the project.

You must manually make folder certs in root of project folder so that certificate will generate there.
You can see default certificate password and name in docker-compose.override.yml file. If you use other you need to change it here too.

You can also use any other certificate just make sure to change it in docker-compose.override.yml file.

You should see something like this in docker desktop when it is finished:
![image](https://github.com/user-attachments/assets/673c0391-4267-4fc6-ad1c-3ffeb4e3440f)

In project folder there is init.sql script that creates database and table with seed data.
You can access database with database tools like DBeaver https://dbeaver.io/ (this is what i used) or something like SQL Server Management Studio
https://learn.microsoft.com/en-us/ssms/download-sql-server-management-studio-ssms

Database is on localhost - sql server authentication, username and password in appsettings.

# Test

To test web service you have swagger UI on https://localhost:5001/swagger/index.html
You will need to authorize it with api key which is in appsettings. You can also test it with other tools like Postman.

![image](https://github.com/user-attachments/assets/57e00c37-8cc9-42b0-b283-82c308c8d9b6)


# Logging
For logging is used Serilog and daily logs are stored in folder logs in project folder.

![image](https://github.com/user-attachments/assets/da85aa80-9e75-4ca3-afcd-8a8f78998985)


All that was tested on Windows.
