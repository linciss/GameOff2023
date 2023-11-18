extends Camera3D

# Adjust these parameters based on your game's needs
var follow_speed : float = 5.0
var delay : float = 0.2
var offset : Vector3 = Vector3(0, 5, -10)

var player : CharacterBody3D
var target_position : Vector3
var interpolation_factor : float = 0.0

func _ready():
	player = get_parent()  # Adjust the path based on your scene structure

func _process(delta):
	# Calculate the target position for the camera
	target_position = player.global_transform.origin + offset

	# Adjust the interpolation factor based on the delay
	interpolation_factor = lerp(interpolation_factor, 1.0, follow_speed * delta * (1.0 - delay))

	# Interpolate the current camera position towards the target position with delay
	var new_position : Vector3 = global_transform.origin.lerp(target_position, interpolation_factor)
	global_transform.origin = new_position

	# Look at the player (optional)
	look_at(player.global_transform.origin, Vector3.UP)
