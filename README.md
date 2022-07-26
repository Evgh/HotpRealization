# HotpRealization
A client-server application that uses the HOTP algorithm for two-factor authentication. 

### HotpServer
The HotpServer project is a web API that stores a database of users, allows to register new users, connect two-factor authentication and log into the application using two-factor authentication.

### Client 
The Client project is a desktop application that accesses the HotpServer web API. It supposedly features business functionality. To protect data in this application, two-factor authentication could be enabled. 

### Authorizer
The Authorizer project serves to perform two-factor HOTP authentication. The user logs into the application using the main account. When you need to log on to the main application, if two-factor authentication is connected, you need to send a confirmation request from the Authorizer. 
