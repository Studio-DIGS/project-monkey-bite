extends Camera3D

# Camera settings
@export var camera_speed = 2.5
@export var look_ahead_distance = 4.0
@export var orientation = 1

func _ready():
	pass

func _physics_process(delta):
	# Slide camera along x axis to get ahead of player
	var target_position = Vector3(look_ahead_distance, 0, 0) * orientation

	# Calculate the camera's position using linear interpolation (lerp) for smooth movement
	var new_position = self.position.lerp(target_position, camera_speed * delta)

	# Set the camera's position to the new calculated position
	self.position = new_position


func _on_player_turn_around():
	orientation *= -1
