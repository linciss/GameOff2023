extends Camera3D

# Properties
var player : Node3D = null
var follow_speed : float = 100
var x_offset : float = -10
var z_offset : float = 10
var y_offset : float = 10

# Variables for delayed movement
var target_position : Vector3

func _ready():
	player = get_parent()

func _process(delta: float) -> void:
	if player != null:
		# Calculate the target position based on the player's position with offsets
		var offset = player.global_transform.basis.z * (z_offset) + player.global_transform.basis.x * (x_offset) + player.global_transform.basis.y * (y_offset)
		target_position = lerp(target_position, player.global_transform.origin + offset, delta * 5)

		# Set the camera's position to the target position
		global_transform.origin = target_position

		# Adjust only the speed factor without directly affecting the position
		var camera_speed = global_transform.basis.z.normalized() * follow_speed * delta
		global_transform.origin += camera_speed.normalized()

# Connect this function to a signal emitted by the player when its position changes
func on_player_position_changed(new_position: Vector3) -> void:
	# Update the target position when the player moves
	target_position = new_position
