extends Camera3D
#FOLLOW PLAYER
@export_category("Follow this node")
@export var camera_point : Node3D   #THE CAMERA WILL FOLLOW THIS NODE
@export_category("Wait Time")
@export var wait_time : float   #How long for the camera to start following the node, can leave it at 0 too
var point_position : Vector3    #The global position of the node we want the camera to follow
var speed := 1.0    #How fast the camera will reach its destination
 
# Called when the node enters the scene tree for the first time.
func _ready():
	set_as_top_level(true)   #Prevents the camera from being glued to the player (won't follow on its own)
 
func _process(delta):
	transform.origin = lerp(transform.origin, camera_point.transform.origin, speed * delta)
