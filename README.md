# Mail sending service
## Description
The MailSenderService application is a basic email sending implementation that takes a pre-configured email and sends it to the recipient.
> This service is one of the many third-party tools commonly found in commercial development.


## Technical requirements
To be able to send an email using MailSenderService, you need to specify the confidential data/parameters required for sending emails in appsettings.json. The list of data:

| Key         | Description                                               |
|-------------|-----------------------------------------------------------|
| Mail        | The Mail-Id from which you would want to send a mail from.|
| DisplayName | The Name that appears to the receiver.                    |
| Password    | Valid password to login to SMTP server.                   |
| Host        | Server URL for SMTP.                                      |
| Port        | Port Number of the SMTP server.                           |

### Where can I obtain these confidential data/parameters?
These details are provided by your SMTP client. It could be SendGrid, Google Workspace, or even a Fake SMTP Provider like Ethereal. Regardless of which one you choose, make sure to specify all the data/parameters in the appsettings.json configuration file. <font color="red">**And remember, publishing confidential data/parameters publicly can be dangerous and may lead to your data being compromised!**</font>

### How to send email?
To send a mail, you must pass the following parameters:

| Key         | Description                                                  |
|-------------|--------------------------------------------------------------|
| ToEmail     | Recipient's email address.                                   |
| Subject     | The subject of the email message.                            |
| Body        | The body content of the email message.                       |
| Attachments | The list of file attachments included with the email message.|

> ToEmail, Subject, Body should be sent as a parameter and attachments in the request body. You can see the curl request below.

``` curl
curl --location 'https://localhost:7019/api/Mail?ToEmail=test.mail%40gmail.com&Subject=Test%20meil%20message&Body=This%20message%20was%20sent%20via%20MailSenderService' \
--header 'accept: */*' \
--form 'Attachments=@"photo_2023-08-23_13-48-58 (2).jpg"'
```

## Used links
**Send Emails with .Net:**
https://codewithmukesh.com/blog/send-emails-with-aspnet-core/