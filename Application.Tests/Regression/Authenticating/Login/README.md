Authenticating :: Login 
=======================

The GeoCMS can only be accessed by authenticated users. During authentication, a username and password is provided and an authentication token is created if the credentials are valid. 
This authentication token should be used for all subsequent interactions with the GeoCMS. 

* Scenario 1: As an anonymous user, I should not be able to obtain an access token with an invalid password

	No authentication token is generated. 
	A response is returned from the server indicating invalid credentials were provided. 

* Scenario 2: As an anonymous user, I should not be able to obtain an access token with an invalid username

	No authentication token is generated. 
	A response is returned from the server indicating invalid credentials were provided. 

* Scenario 3: As an anonymous user, I should not be able to obtain an access token with providing any credentials

	No authentication token is generated. 
	A response is returned from the server indicating invalid credentials were provided. 

* Scenario 4: As an anonymous user, I should be able to obtain an access token by providing valid credentials

	An authentication token is generated. 
	The token is returned in the response from the server. 