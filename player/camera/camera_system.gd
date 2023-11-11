extends Node3D
class_name CameraSystem;


@export var target : Node3D;

signal change_direction;

# Called when the node enters the scene tree for the first time.
func _ready():
	$Follow_Cam.target = self.target;

func _on_player_turn_around():
	emit_signal("change_direction");
