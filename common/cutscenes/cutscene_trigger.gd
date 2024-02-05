extends Area3D

@export var cutscene: CutsceneResource

func _on_body_entered(body: Player):
	GameManager.emit_signal("start_cutscene", cutscene)
	set_deferred("monitoring", false)
