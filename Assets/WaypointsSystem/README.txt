WAYPOINTS PATH SYSTEM
Basic manual

Please check additional detail in the _MANUAL.pdf (in .\WaypointsSystem\_EasyWaypointSystem_\)

Overview
Universal and powerful waypoint system allows you to create waypoints and path easily and even in run-time. You can use it for any situations where path/waypoints following are needed: 
•	Moving platforms
•	AI movement and patrolling NPCs behavior
•	Obstacle avoidance 
•	Animated environment objects
•	etc.

System supports:
•	Dynamic path changing (i.e. even characters can be turned into waypoints)
•	Paths intersecting and common/shared waypoints
•	Different looping types
•	Different facing /movement types
•	Dynamic speed changing (when the waypoint is reached)
•	Custom functions calling (when the waypoint is reached)
•	Dynamic preventing of  collision with mooving obstacles etc.

Moreover you can trigger delays and any other actions/functions right into waypoint!
This system works on all platforms supported by Unity3D.



How to use
To use Waypoints path system – you should just:
1.	Assign WaypointHolder script to any gameObject
2.	Create several waypoints (using prefab or just assign Waypoint script to any gameObject)
3.	Assign waypoints to list in WaypointHolder  or just make WaypointHolder  parent to them (in this case path will be created automatically according to waypoints order in Hierarchy)
4.	Attach WaypointMover script to any gameObject you want to move along path (and assign WaypointHolder to its related property).
5.	If needed - tune movement parameters… and….   Enjoy!





Please don’t hesitate to contact me in any reason by mail: AllebiGames@gmail.com
