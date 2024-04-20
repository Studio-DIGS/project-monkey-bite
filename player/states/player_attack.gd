extends PlayerState

var combo_counter = 0
var max_combo = 0
var combo: Array[AttackResource]
var apply_gravity = false
var contact = false 
var air: bool

func enter(msg := {}):
	if msg.get('air', false) == true:
		air = true
		if player.is_armed: combo = player.air_combo
		else: combo = player.air_kick
	else:
		air = false
		if player.is_armed: combo = player.combo
		else: combo = player.punch
	
	# set the current attack to the index of the player's combo
	# declare max combo index
	var curr_attack = combo[combo_counter]
	max_combo = combo.size() - 1
#	print(combo_counter)

	# configure hitbox to correct AttackResource
	curr_attack.knockback.x = abs(curr_attack.knockback.x) # this seems hacky but it works
	curr_attack.knockback.x *= player.orientation
	player.hitbox.configure_hitbox(curr_attack)
	
	# plays the initial animation
	# the rest will be queued
	if combo_counter == 0:
		player.anim.play(curr_attack.animation)
		

func physics_update(delta):
	if apply_gravity:
		player.velocity.y -= player.gravity * delta
	
	# handle movement
	var hori_velocity = combo[combo_counter].movement.x * player.orientation
	player.velocity.x = lerp(player.velocity.x, hori_velocity, delta * player.accel)
	player.velocity.y = lerp(player.velocity.y, 0.0, delta * player.accel)
	player.move_and_slide()
	
#	# animation cancel
#	if player.try_throw and player.is_armed:
#		player.anim.clear_queue()
#		state_machine.transition_to("Throw")
	
	# queue the next attack when the player presses the attack button
	# if combo doesn't exceed the max and there's no attack animations in queue.
	if combo_counter < max_combo and not player.anim.get_queue():
		if player.try_attack:
			combo_counter += 1
			contact = false
			player.anim.queue(combo[combo_counter].animation)


func _on_animation_player_animation_finished(_anim_name):
	if state_machine.state.name == "Attack":
		if air: player.anim.play("Fall")
		else: player.anim.play("Idle")
		FMODRuntime.play_one_shot_attached_path("event:/Sword Whiff", self)
		# wait for animation to finish blending to idle before fully transitioning state
		var blend_time = player.anim.playback_default_blend_time
		await get_tree().create_timer(blend_time).timeout
		
		combo_counter = 0
		if air: state_machine.transition_to("Air") 
		else: state_machine.transition_to("Idle")

func _on_animation_player_animation_changed(_old_name, _new_name):
	if state_machine.state.name == "Attack":
		state_machine.transition_to("Attack", {air = air})
		FMODRuntime.play_one_shot_attached_path("event:/Sword Whiff", self)

# check if made contact with enemy
func _on_hitbox_area_entered(_area):
	contact = true
	FMODRuntime.play_one_shot_attached_path("event:/Sword Slashes", self)
