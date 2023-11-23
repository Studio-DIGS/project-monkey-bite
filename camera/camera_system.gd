extends SubViewportContainer
class_name CameraSystem; 

@export_group("Dependencies")
@export var target : Node3D;

signal change_direction;

# Called when the node enters the scene tree for the first time.
func _ready():
	$Follow_Cam.set_target(self.target);

func _on_player_turn_around():
	emit_signal("change_direction");
