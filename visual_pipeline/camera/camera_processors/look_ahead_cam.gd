extends CameraProcesser

# Camera settings
@export var camera_speed : float = 2.5;
@export var look_ahead_distance : float  = 2.0;

func process_cam(dir : float, current_position : Vector3, delta : float, _camera : Camera3D) -> Vector3:
	# Slide camera along x axis to get ahead of player
	var target_position = current_position + dir * Vector3(look_ahead_distance, 0, 0);

	# Calculate the camera's position using linear interpolation (lerp) for smooth movement
	current_position = current_position.lerp(target_position, camera_speed * delta);
	
	return current_position;
