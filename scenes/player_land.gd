extends PlayerState

func enter(_msg := {}):
	if is_equal_approx(player.velocity.x, 0.0):
		state_machine.transition_to("Idle")
	else:
		state_machine.transition_to("Run")
