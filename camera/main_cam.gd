extends PixelCamBase
class_name MainCamera

# Camera Parent node that moves the camera
@export var camera_move_container : Node3D;
@export var camera_follow : CameraProcesser;
@export var camera_lookahead : CameraProcesser;

var current_pos : Vector3 = Vector3(0,0,0);

var dir : float = 1;

func setup():
	super.setup()

func _physics_process(delta):
	current_pos = camera_follow.process_cam(dir, current_pos, delta);
	current_pos = camera_lookahead.process_cam(dir, current_pos, delta);
	
	# snap
	var snap_result = get_pixel_snapped_pos(current_pos, camera_move_container.basis)
	update_display(snap_result)
	
	# update position
	camera_move_container.global_position = snap_result.snapped_world_pos
	

func _on_camera_system_change_direction():
	dir *= -1;
