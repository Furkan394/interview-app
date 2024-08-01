# Interview App

The Interview Application is designed as a platform where communication students at the university can upload and share their interview assignments, and instructors can review these interviews.

Although the project's requirements are not very complex, I chose to use a microservices architecture to develop this project individually to advance my skills in advanced software development.

### Technologies and Tools
:star: .NET Core 8<br>
:star: ASP.NET Web API<br>
:star: EntityFramework Core<br>
:star: PostgreSQL<br>
:star: MongoDB<br>
:star: Docker<br>
:star: Duende Identity Server<br>
:star: Postman<br>
:star: RabbitMQ<br>

### Architecture

InterviewService: This service allows users to upload and edit interviews in the system.
<br>
SearchService: This service is used to query all interviews.
<br>
IdentityService: The service used for authentication and user login.
<br>
GatewayService: This service facilitates communication between services.
<br>
RabbitMQ: The messaging system ensures consistency between the databases of the InterviewService and the SearchService.

### Front-End
In development with NextJS
