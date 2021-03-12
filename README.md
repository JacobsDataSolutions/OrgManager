# OrgManager
OrgManager is a demo enterprise, multi-tenant SaaS application built using .NET 5 (.NET Core) and Angular. The purpose of this project is to demonstrate Clean Domain-Driven Design (Clean DDD) and Command/Query Responsibility Segregation (CQRS) concepts, along with other architectural patterns and design best practices.

## Philosophy
The architectural approach demonstrated is for illustrative purposes and may or may not be suitable for all applications. In particular, this architecture emphasizes a separation between Domain entities and Persistence entities, facilitated by a mapping infrastructure. This distinguishes it from many other Clean DDD implementations--especially those built using Entity Framework--which make no such distinction.

I will add more documentation as the solution evolves. Polite comments and feedback are welcome.

## Blog Series
This application accompanies an in-depth blog series discussing multi-tenant SaaS, Clean DDD, and CQRS concepts at [Software Alchemy](https://blog.jacobsdata.com/).

## Playing Around
By default, the application *should* scaffold the database and populate it with dummy data. It will create dummy user accounts that you can use to log in and play around with.

The dummy user accounts have names like
* TEST-CUSTOMER@ORG-MANAGER.COM
* TEST-EMPLOYEE1@ORG-MANAGER.COM
* TEST-EMPLOYEE2@ORG-MANAGER.COM
* so on, and so forth...

For all of these accounts, the default password is "P@ssw0rd1".

Routes to screens in the app that are (semi) functional at the moment are:

/
/lets-get-started
/employee-registered
/customer/update
/customer/manage-tenants
/t/*tenant slug*/
/t/*tenant slug*/employee
/t/*tenant slug*/employee/update
/test
/unauthorized

^^^ In the above routes, replace "tenant slug" with the actual slug for the tenant that the test user has access to, for example, "test-tenant-10".

Thank you for your interest, and happy coding...

## Screen Mockups

### Front Page
![](images/Front-Page.png?raw=true)

### Registration Main
![](images/Registration-Main-Page.png?raw=true)

### Employee Registration
![](images/Employee-Registration.png?raw=true)

### Employer Registration
![](images/Employer-Registration.png?raw=true)

### Registration Complete
![](images/Registration-Complete.png?raw=true)

### Add/Edit Customer Information
![](images/Add-Edit-Customer-Information.png?raw=true)

### Add/Update Tenants
![](images/Customer-Add-Update-Tenants.png?raw=true)

### Add/Edit Employee Information
![](images/Add-Edit-Employee-Information.png?raw=true)

### Submit PTO
![](images/Employee-Home-PTO-Subscreen.png?raw=true)

### Submit Org Feedback
![](images/Employee-Home-Org-Feedback-Subscreen.png?raw=true)

### Upgrade/Downgrade/Cancel
![](images/Upgrade-Downgrade-Cancel.png?raw=true)

### Help/Support
![](images/Help-Support.png?raw=true)

### Privacy Policy
![](images/Privacy-Policy.png?raw=true)

### About
![](images/About.png?raw=true)

### Terms and Conditions
![](images/Terms-and-Conditions.png?raw=true)

### Testimonials
![](images/Testimonials.png?raw=true)
