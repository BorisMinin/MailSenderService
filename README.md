# Mail sending service.
## Description
The MailSenderService application is a basic email sending implementation that takes a pre-configured email and sends it to the recipient.
> This service is one of the many third-party tools commonly found in commercial development.

## Technical requirements
To be able to send an email using MailSenderService, you need to specify the confidential data/parameters required for sending emails. The list of data:

| Key         | Description                                               |
|-------------|-----------------------------------------------------------|
| Mail        | The Mail-Id from which you would want to send a mail from |
| DisplayName | The Name that appears to the receiver                     |
| Password    | Valid password to login to SMTP server                    |
| Host        | Server URL for SMTP                                       |
| Port        | Port Number of the SMTP server                            |

### Where can I obtain these confidential data/parameters?
These details are provided by your SMTP client. It could be SendGrid, Google Workspace, or even a Fake SMTP Provider like Ethereal. Regardless of which one you choose, make sure to specify all the data/parameters in the appsettings.json configuration file. <font color="red">**And remember, publishing confidential data/parameters publicly can be dangerous and may lead to your data being compromised!**</font>