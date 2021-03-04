## Authentication vs. Authorization

Consider a person walking up to a locked door to provide care to a pet while the family is away on vacation. That person needs:

- **Authentication**, in the form of a key. The lock on the door only grants access to someone with the correct key in much the same way that a system only grants access to users who have the correct credentials.
- **Authorization**, in the form of permissions. Once inside, the person has the authorization to access the kitchen and open the cupboard that holds the pet food. The person may not have permission to go into the bedroom for a quick nap. 

Authentication and authorization work together in this example. A pet sitter has the right to enter the house (authentication), and once there, they have access to certain areas (authorization).


| |**Authentication**|**Authorization**| 
|-|-|-|
|**What does it do?**|Verifies credentials |Grants or denies permissions|
|**How does it work?**|Through passwords, biometrics, one-time pins, or apps|Through settings maintained by security teams|
|**Is it visible to the user?**|Yes|No|
|**It is changeable by the user?**|Partially|No |
|**How does data move?**|Through ID tokens|Through access tokens |



 

Systems implement these concepts in the same way, so it’s crucial that IAM administrators understand how to utilize both:

- **Authentication**. Let every staff member access your workplace systems if they provide the right credentials in response to your chosen authentication requirements.
- **Authorization**. Grant permission to department-specific files, and reserve access to confidential data, such as financial information, as needed. Ensure that employees have access to the files they need to do their jobs. 

Understand the difference between authentication and authorization, and implement IAM solutions that have strong support for both. You will protect your organization against data breaches and enable your workforce to be more productive.

[source](https://www.okta.com/identity-101/authentication-vs-authorization/)

---

