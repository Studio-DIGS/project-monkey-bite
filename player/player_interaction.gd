extends Area3D

signal swap_swords(sword: SwordBody)

func _on_body_entered(body):
	if body is SwordBody:
		emit_signal("swap_swords", body)
