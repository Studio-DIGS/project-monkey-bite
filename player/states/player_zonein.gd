extends PlayerState

var attack: AttackResource
var apply_gravity = false
var contact = false 
var zone_velocity = 10.0

func enter(_msg := {}):
	attack = player.zone_in

	# configure hitbox to correct AttackResource
	attack.knockback.x = abs(attack.knockback.x) # this seems hacky but it works
	attack.knockback.x *= player.orientation
	player.hitbox.configure_hitbox(attack)
	
	player.anim.play(attack.animation)

func physics_update(delta):
	if apply_gravity:
		player.velocity.y -= player.gravity * delta
	
	# handle movement
	player.velocity.x = lerp(player.velocity.x, zone_velocity * player.orientation, delta * player.accel)
	player.velocity.y = lerp(player.velocity.y, 0.0, delta * player.accel)
	player.move_and_slide()
	
	# queue the next attack
	if contact and not player.anim.get_queue():
		if player.try_attack:
			contact = false
			player.anim.queue(player.combo[0].animation)



func _on_animation_player_animation_finished(_anim_name):
	state_machine.transition_to("Idle")

func _on_animation_player_animation_changed(_old_name, _new_name):
	state_machine.transition_to("Attack")

# check if made contact with enemy
func _on_hitbox_area_entered(_area):
	contact = true
