extends PlayerState

func enter(_msg := {}):
	pass

func physics_update(delta):
	pass



func _on_animation_player_animation_finished(_anim_name):
	state_machine.transition_to("Idle")

func _on_animation_player_animation_changed(_old_name, _new_name):
	state_machine.transition_to("Attack")
