extends Area3D

signal swap_swords(sword: Sword)

func _on_body_entered(body):
	if body is SwordBody:
		emit_signal("swap_swords", body.stats)
		body.queue_free()
