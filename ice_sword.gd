extends Node3D
var area_sword
var hitbox_duration
var is_special_visible
var is_special_ready: bool
var cooldown_timer

var rotation_origin

#NOTES MAKE SURE TO CONNECT PLAYER "turn_around" to this script to update orientation of ice blade
func _ready():
	rotation_origin = $"Rotation Origin"
	area_sword = $"Rotation Origin/Area3D"
	is_special_visible = $"Rotation Origin/Area3D/MeshInstance3D"
	is_special_ready = true
	disable_sword_hurtbox()
	hitbox_duration = .01 #Edit this for making ice sphere last longer on map
	
	cooldown_timer = $"Cooldown Timer"


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("sword special"):
		if is_special_ready: #Insert attack animation here Jacob
			enable_sword_hurtbox()
			await get_tree().create_timer(hitbox_duration).timeout
			disable_sword_hurtbox()
			is_special_ready = false
			cooldown_timer.start()
		else:
			print("Special On Cooldown")
			

func enable_sword_hurtbox():
	is_special_visible.visible = true
	area_sword.monitoring = true
	area_sword.monitorable = true

func disable_sword_hurtbox():
	is_special_visible.visible = false
	area_sword.monitoring = false
	area_sword.monitorable = false

func _on_cooldown_timer_timeout():
	is_special_ready = true
