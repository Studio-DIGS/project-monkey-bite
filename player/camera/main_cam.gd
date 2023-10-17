extends Node

# Pixel grid locking
@export var texelWidth = 320;
@export var texelHeight = 180;

# Camera Parent node that moves the camera
@export var camera_move_container : Node3D;

@export var camera : Camera3D;
@export var camera_follow : CameraProcesser;
@export var camera_lookahead : CameraProcesser;


var current_pos : Vector3 = Vector3(0,0,0);

var screenResolution : Vector2;
var xStepSize : float;
var yStepSize : float;

var dir : float = 1;

func _ready():
	_calc_pixel_grid();

	
func _calc_pixel_grid():
	var width = ProjectSettings.get_setting("display/window/size/viewport_width");
	var height = ProjectSettings.get_setting("display/window/size/viewport_height");		
	screenResolution = Vector2(width, height);
	
	print(screenResolution);
	
	var vp_size = Vector2(camera.size * width/height, camera.size);
	xStepSize = vp_size.x / texelWidth;
	yStepSize = vp_size.y / texelHeight;
	
	var tilt = camera.transform.basis.get_euler().x;
	print(tilt / PI * 180);
	
	yStepSize /= cos(tilt); 
	
	print(xStepSize);
	print(yStepSize);
	

func _physics_process(delta):
	current_pos = camera_follow.process_cam(dir, current_pos, delta);
	current_pos = camera_lookahead.process_cam(dir, current_pos, delta);
	
	# Pixel grid locking using global space
	var snappedX = floor(current_pos.x / xStepSize) * xStepSize;
	var snappedY = floor(current_pos.y / yStepSize) * yStepSize;
	
	# snap offset in global space
	var offset = Vector3(snappedX - current_pos.x, snappedY - current_pos.y, 0);
	
	# Set the camera's position to the new calculated position
	camera_move_container.position = current_pos + offset;

func _on_camera_system_change_direction():
	dir *= -1;
