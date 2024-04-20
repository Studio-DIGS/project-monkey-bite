extends PlayerState

var attack_tries = 1
var jump_force: float
var jumping = false

func enter(msg := {}):
	jump_force = 0.0
	if msg.has("do_jump"):
		player.anim.play("Jump")
		FMODRuntime.play_one_shot_attached_path("event:/Player Jump", self)
		player.velocity.y = player.min_jump_height
		jumping = true
		player.anim.queue("Fall")
	else:
		player.anim.play("Fall")


func physics_update(delta):
	if jumping:
		if player.jump_time > 0.0 and jump_force < player.max_jump_height:
			jump_force = min(player.jump_time * player.jump_force_coefficient, player.max_jump_height)
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
	
	if player.stagger:
		state_machine.transition_to("Stagger")
		return
	
	if attack_tries > 0 and player.try_attack:
		attack_tries -= 1
		state_machine.transition_to("Attack", {air = true})
	
	elif player.try_special and player.is_armed:
		state_machine.transition_to("Special")
	
	elif player.try_interact:
		player.interaction_area.set_deferred("monitorable", true)
		player.interaction_area.set_deferred("monitoring", true)
	
	elif player.try_dash:
		state_machine.transition_to("Dash")
	
	# Landing
	elif player.is_on_floor():
		player.stop_jump()
		jumping = false
		attack_tries = 1 # reset number of attempts at an air attack
		state_machine.transition_to("Land")
	
	player.reorient()
