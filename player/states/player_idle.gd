extends PlayerState

func enter(_msg := {}):
	if player.is_armed:
		player.anim.play("Idle_Armed")
	else:
		player.anim.play("Idle_Unarmed")

func physics_update(delta):
	player.velocity.x = lerp(player.velocity.x, 0.0, delta * player.accel)
	player.velocity.y = lerp(player.velocity.y, 0.0, delta * player.accel)
	player.move_and_slide()
	
	if player.stagger:
		state_machine.transition_to("Stagger")
		return
	
	if not player.is_on_floor():
		state_machine.transition_to("Air")
		return
	
	if player.try_jump:
		state_machine.transition_to("Air", {do_jump = true})
	
	elif player.hori_input != 0.0:
#		player.anim.play("Run")
		state_machine.transition_to("Run")
	
	elif player.try_dash:
		state_machine.transition_to("Dash")
	
	elif player.try_attack:
		state_machine.transition_to("Attack")
	
	elif player.try_throw and player.is_armed:
		state_machine.transition_to("Throw")
	
	elif player.try_special and player.is_armed:
		state_machine.transition_to("Special")
	
	elif player.try_interact:
		player.interaction_area.set_deferred("monitorable", true)
		player.interaction_area.set_deferred("monitoring", true)
	
#	elif player.try_special and player.is_armed:
#		state_machine.transition_to("Special")
	
	player.reorient()
