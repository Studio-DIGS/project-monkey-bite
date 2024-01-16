extends PlayerState

var combo_counter = 0
var max_combo = 0
var combo: Array[AttackResource]
var apply_gravity = false
var orientation = 1
var contact = false 

func enter(msg := {}):
	combo = player.combo
	
	# set the current attack to the index of the player's combo
	# declare max combo index
	var curr_attack = combo[combo_counter]
	max_combo = combo.size() - 1
	print(combo_counter)
	
	# configure hitbox to correct AttackResource
	curr_attack.knockback.x *= orientation
	player.hitbox.configure_hitbox(curr_attack)
	
	# plays the initial animation
	# the rest will be queued
	if combo_counter == 0:
		player.anim.play(curr_attack.animation)

func physics_update(delta):
	if apply_gravity:
		player.velocity.y -= player.gravity * delta
	
	# handle movement
	player.velocity.x = lerp(player.velocity.x, 0.0, delta * player.accel)
	player.velocity.y = lerp(player.velocity.y, 0.0, delta * player.accel)
	player.move_and_slide()
	
	# queue the next attack
	if combo_counter < max_combo and contact and not player.anim.get_queue():
		if Input.is_action_just_pressed("attack"):
			combo_counter += 1
			contact = false
			player.anim.queue(player.combo[combo_counter].animation)



func _on_animation_player_animation_finished(_anim_name):
	combo_counter = 0
	state_machine.transition_to("Idle")

func _on_animation_player_animation_changed(old_name, new_name):
	state_machine.transition_to("Attack")


func _on_player_turn_around():
	orientation *= -1

# check if made contact with enemy
func _on_hitbox_area_entered(area):
	contact = true
