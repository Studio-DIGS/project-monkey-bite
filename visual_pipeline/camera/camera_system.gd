extends SubViewportContainer
class_name CameraSystem; 

@export_group("Dependencies")
@export var target : Node3D;
@export var follow_cam : FollowCameraProcesser;

signal change_direction;

# Called when the node enters the scene tree for the first time.
func _ready():
	follow_cam.set_target(self.target);

func _on_player_turn_around():
	emit_signal("change_direction");
