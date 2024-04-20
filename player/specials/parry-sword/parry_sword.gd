extends Node3D
var area_sword
var shield_duration
var is_shield_visible
var is_counter_ready: bool

signal force_player_attack
# Called when the node enters the scene tree for the first time.
func _ready():
	area_sword = $Area3D
	is_shield_visible = $Area3D/MeshInstance3D
	is_counter_ready = true
	disable_sword()
	shield_duration = 1


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("sword special"):
		enable_sword()
		await get_tree().create_timer(shield_duration).timeout
		if is_counter_ready:
			force_attack()
		disable_sword()

func enable_sword():
	is_shield_visible.visible = true
	area_sword.monitoring = true
	area_sword.monitorable = true

func disable_sword():
	is_shield_visible.visible = false
	area_sword.monitoring = false
	area_sword.monitorable = false
	
func force_attack():
	emit_signal("force_player_attack")

