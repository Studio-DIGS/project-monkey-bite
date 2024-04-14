extends PlayerState

func enter(_msg := {}):
	player.anim.play("Throw")

func physics_update(delta):
	pass



func _on_animation_player_animation_finished(_anim_name):
	state_machine.transition_to("Idle")

