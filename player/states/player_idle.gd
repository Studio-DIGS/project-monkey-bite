extends PlayerState

func enter(_msg := {}):
	player.anim.queue("Idle")

func physics_update(delta):
	player.velocity.x = lerp(player.velocity.x, 0.0, delta * player.accel)
	player.velocity.y = lerp(player.velocity.y, 0.0, delta * player.accel)
	player.move_and_slide()
	
	if not player.is_on_floor():
		state_machine.transition_to("Air")
		return
	
	if player.try_jump:
		state_machine.transition_to("Air", {do_jump = true})
	
	elif player.hori_input != 0.0:
		player.anim.play("Run")
		state_machine.transition_to("Run")
	
	elif player.try_attack:
		state_machine.transition_to("Attack")
