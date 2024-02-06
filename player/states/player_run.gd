extends PlayerState

func enter(_msg := {}):
	player.anim.play("bob")

func physics_update(delta):
	if not player.is_on_floor():
		state_machine.transition_to("Air")
		return
	
	player.velocity.x = lerp(player.velocity.x, player.hori_input * player.speed, delta * player.accel)
	player.move_and_slide()
	
	if player.try_jump:
		state_machine.transition_to("Air", {do_jump = true})
	
	elif player.try_attack:
		print("test1")
		player.zone_in_dist.enabled = true
		player.stay_put_dist.enabled = true
		
		if player.zone_in_dist.is_colliding() and not player.stay_put_dist.is_colliding():
			player.zone_in_dist.enabled = false
			player.stay_put_dist.enabled = false
			state_machine.transition_to("ZoneIn")
		else:
			player.zone_in_dist.enabled = true
			player.stay_put_dist.enabled = true
			print("test2")
			state_machine.transition_to("Attack")
	
	elif player.hori_input == 0.0:
		state_machine.transition_to("Idle")
