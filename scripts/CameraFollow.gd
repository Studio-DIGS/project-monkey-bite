extends Camera3D

# Camera settings
@export var target: Node3D
@export var camera_speed = 5.0  # Adjust this value to control the camera's follow speed
@export var offset: Vector3 = Vector3(0, 0, 10)
@export var lerp_speed: float = 0.1

func _ready():
	# Ensure the camera starts at a reasonable initial position
	self.transform.origin = Vector3(0, 5, 5)  # Adjust the initial camera position

func _process(delta):
	if target != null:
		# Get the target position (taking into account distance camera should be away in z)
		var target_position = target.global_position + offset

		# Calculate the camera's position using linear interpolation (lerp) for smooth movement
		var new_position = self.global_position.lerp(target_position, camera_speed * delta)

		# Set the camera's position to the new calculated position
		self.position = new_position
	else:
		print("Target reference not set. Set it in the inspector.")
