extends PlayerState

func enter(_msg := {}):
	player.anim.play("bob")

func physics_update(delta):
	player.velocity.x = lerp(player.velocity.x, 0.0, delta * player.accel)
	player.velocity.y = lerp(player.velocity.y, 0.0, delta * player.accel)
	player.move_and_slide()
	
	if not player.is_on_floor():
		state_machine.transition_to("Air")
		return
	
	if Input.is_action_just_pressed("jump"):
		state_machine.transition_to("Air", {do_jump = true})
	
	elif player.hori_input != 0.0:
		state_machine.transition_to("Run")
	
	elif Input.is_action_pressed("attack"):
		state_machine.transition_to("Attack")
