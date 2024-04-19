extends Node3D

@onready var smear = preload("res://player/swords/sword_smear.tscn")

func _ready():
	var smear_instance: Trail3D = smear.instantiate()
	smear_instance.position = self.global_position * -1
	self.add_child(smear_instance)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	self.global_position = GameManager.camera.project_position(get_viewport().get_mouse_position(),10);
