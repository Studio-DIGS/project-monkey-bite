extends PlayerState

func enter(_msg := {}):
	player.anim.play("Land")
	FMODRuntime.play_one_shot_attached_path("event:/Player Land", self)
	if is_equal_approx(player.velocity.x, 0.0):
		state_machine.transition_to("Idle")
	else:
		state_machine.transition_to("Run")
		
