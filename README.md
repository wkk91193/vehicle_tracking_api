# Vehicle Tracking Service

This solution will to be able to track vehicles position using GPS navigation. A device emboarded in a vehicle, will communicate with this API to register the vehicle and update its position.

## Architechure


 ![High level architechure](https://github.com/wkk91193/vehicle_tracking_api/blob/master/VehicleTracking_Api/Images/VehicleTracking.jpg) 

The API gateway will recieve requests from the any client facing Application and direct them to their respective API endpoint where it will processed. Identity and Role information will be stored on the SQL Database, while the Vehicle and it's location information will stored on the Non relataional (Cosmos Databases)


## Considerations

1. Scalability: There will be 10,000 vehicles equipped with the device recording their location every 30 seconds. We need to ensure the solution is scalable and the database correctly designed for that amount of records.

2. Extensibility: If the customer wants to store more properties (fuel, speed, etc.). How do we extend the data model to support it?

3. Security: We need to ensure a device or user cannot update the position of another vehicle .

## Main highlights

Keeping the considerations in mind, this solution was developed.

_To ensure scalability and ease of extending the database solution for future a non relational database was selected. Unlike a traditional SQL database, it's needless to update the schema eachtime. However I had to make use of SQL databases for storing user registration and roles information_

_For security I ensured encoding user registration email,role infomation(user/admin) vehicle registration into JWT tokens,which will be used on subsequent requests to authenticate a user or an admin_ 

In addittion I have implemented the following.

1. API Gateway: In order provide security to the API considering overuse and abuse and for more control over monitoring the usage.This also comes with a developer portal experience  so that the developer can test the APIs on the browser prior integration.

2. CI/CD pipeline: Using Github actions all the changes I pushed to repository will be build and it's binaries willl deployed to the API directly cutting any manual work of publishing APIs and identifying problems early.

3. Logging/Monitoring: I have enabled the use log streaming and application insights to monitor all incoming requests, thereby easier for investigating any failures or anomalies.

4. Validation: Using Fluent validation APIs to validate all incoming request bodies.

5. API documentation: For a seemless integration experience, API documentation is valuable. I have enabled Swagger API documentation for all requests.

## API Gateway Developer Portal

https://dev-vehicletrackingapim.developer.azure-api.net/

_Note: Click on Explore API button to see the list of the APIs_


## How to run locally.

0.Please copy and replace the appSetting.json file provide via email.

1. Open the solution on Visual Studio 2019 or later.

2. Build the solution.

3. Run solution.

4. Use swagger Try out option/Postman rest client to test locally.

5. For security concerns only an existing admin user can create another admin user, to check the credentials of an existing admin user please refer to appSettings.json file 

