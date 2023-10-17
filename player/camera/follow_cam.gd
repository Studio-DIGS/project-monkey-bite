extends CameraProcesser

# Camera settings
@export var target: Node3D

@export var camera_speed : float = 5.0
@export var catchup_speed : float = 2.5
@export var catchup_sens : float = 5.0
@export var vertical_offset : float = 4.0
@export var depth_offset : float = 4.0
@export var lerp_speed : float = 0.1

var speed = camera_speed

var currentPosition : Vector3;
	

func _ready():
	# Ensure the camera starts at a reasonable initial position
	currentPosition = Vector3(0, 5, 5)  # Adjust the initial camera position
	
func process_cam(_dir : float, current_position : Vector3, delta : float):
	if target != null:
		# Get the target position (taking into account distance camera should be above the target)
		var target_position = target.global_position + Vector3(0, vertical_offset, depth_offset)

		# Calculate the camera's position using linear interpolation (lerp) for smooth movement
		var new_position = current_position.lerp(target_position, camera_speed * delta)

		# Set the camera's position to the new calculated position
		currentPosition = new_position
	
	else:
		print("Target reference not set. Set it in the inspector.")
		
	return currentPosition;

