extends SubViewportContainer
class_name CameraSystem; 

@export_group("Dependencies")
@export var target : Node3D
@export var follow_cam : FollowCameraProcesser

signal change_direction
signal change_target(target)

# Called when the node enters the scene tree for the first time.
func _ready():
	_on_change_target()

# default to the export var at the top
func _on_change_target(target = self.target):
	follow_cam.set_target(target)

func _on_player_turn_around():
	emit_signal("change_direction")
