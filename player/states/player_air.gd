extends PlayerState

var attack_tries = 1
var jump_force: float
var jumping = false

func enter(msg := {}):
	jump_force = 0
	if msg.has("do_jump"):
		player.anim.play("Jump")
		player.velocity.y = player.min_jump_height
		jumping = true
		player.anim.queue("Fall")
	else:
		player.anim.play("Fall")


func physics_update(delta):
	if jumping:
		if player.jump_time > 0.0 and jump_force <= player.max_jump_height:
			jump_force = max(player.jump_time * player.jump_force_coefficient, player.min_jump_height)
			player.velocity.y = jump_force
		else:
			jumping = false
	else:
		jump_force = 0.0
	
	# Apply gravity
	player.velocity.y -= player.gravity * delta
	
	# Horizontal movement
	player.velocity.x = lerp(player.velocity.x, player.hori_input * player.speed, delta * player.accel)
	player.move_and_slide()
	
	if attack_tries > 0 and player.try_attack:
		attack_tries -= 1
		state_machine.transition_to("Attack", {air = true})
	
	# Landing
	elif player.is_on_floor():
		player.stop_jump()
		jumping = false
		attack_tries = 1 # reset number of attempts at an air attack
		state_machine.transition_to("Land")
	
	player.reorient()
