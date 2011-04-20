Make sure you change these values in your web.config or app.config:

<appSettings>
	<add key="Environment" value="dev"/>
	<add key="BatchBookApiKey" value="your_api_key"/>
	<add key="BatchBookSubdomain" value="your_subdomain"/>
</appSettings>

The "BatchBookApiKey" setting sets the default API key to be used if no API key is supplied on the method call.

"BatchBookSubdomain" should be the subdomain your batchbook account runs under.

e.g. "yourdomain" in https://yourdomain.batchbook.com/account/login