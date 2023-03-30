# SVTRobotics

Installation and Running
Application was built with Visual Studio. Download the code and open the solution and run. Use Postman or Swagger to make POST requests.

Improvements:
1) Should have unit tests
2) I attempted to avoid running in O(n) time by only pulling the robots within 10 units from the load. With large numbers of bots, this solution would be faster. However, performance testing would need to be done to prove it out.
3) The sort method hasn't been tested to see if it will be performant with large numbers of bots.
4) There is no support for routing - perhaps there are walls or equipment blocking the path back to the load.
5) There is no anticipated energy needs for the load request. Heavier loads should require more battery drain and would affect the selection of a bot. What if the bot with the highest battery level within the 10 unit distance is 1? 
6) Given #5 there should be a minimum battery level to be able to select the bot
7) Should put the URL and the distance in a dynamic configuration system. The config file in the short term. A better solution is a dynamic, database-driven parameter system.
