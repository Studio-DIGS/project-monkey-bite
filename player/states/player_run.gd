extends PlayerState

func enter(_msg := {}):
	player.anim.play("bob")

func physics_update(delta):
	if not player.is_on_floor():
		state_machine.transition_to("Air")
		return
	
	player.velocity.x = lerp(player.velocity.x, player.hori_input * player.speed, delta * player.accel)
	player.move_and_slide()
	
	if Input.is_action_just_pressed("jump"):
		state_machine.transition_to("Air", {do_jump = true})
	
	elif Input.is_action_pressed("attack"):
		state_machine.transition_to("Attack")
	
	elif is_equal_approx(player.velocity.x, 0.0):
		state_machine.transition_to("Idle")
