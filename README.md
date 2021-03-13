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

* /
* /lets-get-started
* /employee-registered
* /customer/update
* /customer/manage-tenants
* /t/*tenant slug*/
* /t/*tenant slug*/employee
* /t/*tenant slug*/employee/update
* /test
* /unauthorized

^^^ In the above routes, replace "tenant slug" with the actual slug for the tenant that the test user has access to, for example, "test-tenant-10".

Thank you for your interest, and happy coding...

## Screen Mockups

### Front Page
![](https://blob.jacobsdata.com/org-manager/Front-Page.png)

### Registration Main
![](https://blob.jacobsdata.com/org-manager/Registration-Main-Page.png)

### Employee Registration
![](https://blob.jacobsdata.com/org-manager/Employee-Registration.png)

### Employer Registration
![](https://blob.jacobsdata.com/org-manager/Employer-Registration.png)

### Registration Complete
![](https://blob.jacobsdata.com/org-manager/Registration-Complete.png)

### Add/Edit Customer Information
![](https://blob.jacobsdata.com/org-manager/Add-Edit-Customer-Information.png)

### Add/Update Tenants
![](https://blob.jacobsdata.com/org-manager/Customer-Add-Update-Tenants.png)

### Add/Edit Employee Information
![](https://blob.jacobsdata.com/org-manager/Add-Edit-Employee-Information.png)

### Submit PTO
![](https://blob.jacobsdata.com/org-manager/Employee-Home-PTO-Subscreen.png)

### Submit Org Feedback
![](https://blob.jacobsdata.com/org-manager/Employee-Home-Org-Feedback-Subscreen.png)

### Upgrade/Downgrade/Cancel
![](https://blob.jacobsdata.com/org-manager/Upgrade-Downgrade-Cancel.png)

### Help/Support
![](https://blob.jacobsdata.com/org-manager/Help-Support.png)

### Privacy Policy
![](https://blob.jacobsdata.com/org-manager/Privacy-Policy.png)

### About
![](https://blob.jacobsdata.com/org-manager/About.png)

### Terms and Conditions
![](https://blob.jacobsdata.com/org-manager/Terms-and-Conditions.png)

### Testimonials
![](https://blob.jacobsdata.com/org-manager/Testimonials.png)
