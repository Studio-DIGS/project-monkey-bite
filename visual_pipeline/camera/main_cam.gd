extends Node
class_name MainCamera

# Camera Parent node that moves the camera
@export_group("Dependencies")
@export var camera : Camera3D;

@export_group("Camera Processors")
@export var camera_move_container : Node3D;
@export var camera_follow : CameraProcesser;
@export var camera_lookahead : CameraProcesser;
@export var camera_pixel_snap : CameraProcesser;

var current_pos : Vector3 = Vector3(0,0,0);

var dir : float = 1;

func setup():
	return

func _physics_process(delta):
	current_pos = camera_follow.process_cam(dir, current_pos, delta, camera);
	current_pos = camera_lookahead.process_cam(dir, current_pos, delta, camera);
	var final_pos = camera_pixel_snap.process_cam(dir, current_pos, delta, camera);
	
	# update position
	camera_move_container.global_position = final_pos;
	

func _on_camera_system_change_direction():
	dir *= -1;
