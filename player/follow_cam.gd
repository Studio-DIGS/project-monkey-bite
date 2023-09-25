extends Node3D

# Camera settings
@export var target: Node3D
@export var camera_speed = 5.0
@export var vertical_offset = 4.0
@export var camera_arm = 4.0

func _ready():
	# Ensure the camera starts at a reasonable initial position
	self.transform.origin = Vector3(0, 5, 5)  # Adjust the initial camera position

func _physics_process(delta):
	if target != null:
		# Get the target position (taking into account distance camera should be above the target)
		var target_position = target.global_position + Vector3(0, vertical_offset, camera_arm)

		# Calculate the camera's position using linear interpolation (lerp) for smooth movement
		var new_position = self.global_position.lerp(target_position, camera_speed * delta)

		# Set the camera's position to the new calculated position
		self.global_position = new_position
	
	else:
		print("Target reference not set. Set it in the inspector.")
