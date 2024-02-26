extends Area3D

@export var cutscene: CutsceneResource

func _on_body_entered(_body):
	GameManager.emit_signal("start_cutscene", cutscene)
	set_deferred("monitoring", false)
