extends PlayerState

func enter(_msg := {}):
	player.velocity = Vector3.ZERO
	player.anim.play("bob")

func physics_update(_delta):
	if not player.is_on_floor():
		state_machine.transition_to("Air")
		return
	
	if Input.is_action_just_pressed("jump"):
		state_machine.transition_to("Air", {do_jump = true})
	
	elif Input.is_action_pressed("left") or Input.is_action_pressed("right"):
		state_machine.transition_to("Run")
	
	elif Input.is_action_pressed("attack"):
		state_machine.transition_to("Attack")
