# organic

1. Create a devops project 
```
https://dev.azure.com/norton-artficier/azfun_organics
```
for now hook it up to github.com to where your function lives
```
https://github.com/fluffy-bunny/organic
```

2. Run the following;
```bash
./000.Setup.sh  
```
This will make sure you are in the right subscription and will create a service connect in your devops project.

NOTE: You will see that the build fails 
```
There was a resource authorization issue: "The pipeline is not valid. Job Deploy: Step input azureSubscription references service connection organics-openhack which could not be found. The service connection does not exist or has not been authorized for use. For authorization details, refer to https://aka.ms/yamlauthz."
```
click the **[Authorize resources]** button.
I am trying to figure out a way to get this all setup via command lines.

