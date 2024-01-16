extends PlayerState

var attacks: int

func enter(msg := {}):
	if msg.has("do_jump"):
		player.velocity.y = player.jump_height


func physics_update(delta):
	# Apply gravity
	player.velocity.y -= player.gravity * delta
	
	# Horizontal movement
	player.velocity.x = lerp(player.velocity.x, player.hori_input * player.speed, delta * player.accel)
	player.move_and_slide()
	
	if attacks > 0 and Input.is_action_pressed("attack"):
		attacks -= 1
		state_machine.transition_to("Attack", {air = true})
	
	# Landing
	elif player.is_on_floor():
		attacks = 1 # reset number of air attacks
		state_machine.transition_to("Land")
