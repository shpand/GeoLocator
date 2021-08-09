# GeoLocator
1. Server is written on ASP NET CORE framework on .net 5 and can be run on any machine and platform (tested on Mac and Windows)
2. Client is just a swagger plugin that allows to make requests on server (here I cheated but I was told that client is not important and can be as simple as possible)
3. Database is load into memory in 10-20ms.
4. Request handling time is fast and generally lower than 1ms.
5. My solution uses structs instead of classes to store geo data. I'm not sure if I would go with this solution in production (at least it would require a thorough testing) because working with structs entails its own restrictions and inconviniences. One of the drawbacks of using struct is that Location struct takes much more memory than recommended 16 bytes of data for structs. One solution for this situation is to pass struct by ref but it's not always possible.

## How to run
1. Make sure that .net 5 framework is installed
2. Open solution and run GeoLocator project.
3. Swagger web page will be opened in your browser where U can make requests to the both endpoints

## What else I would do if I had more time?
1. Write Unit tests to check that returned data is valid and nothing is missed.
2. Properly load test this project using third party frameworks.
3. Check performance of the server when using classes (struct vs class vs ref struct)