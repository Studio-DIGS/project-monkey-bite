extends Node3D

var transparent_bodies
var body0
var body1
var body2
var body3
var transparency_interval_timer
var transparent_convert: bool
# Called when the node enters the scene tree for the first time.
func _ready():
#	body0 = $"Body Fade 0"
#	body1 = $"Body Fade 1"
#	body2 = $"Body Fade 2"
#	body3 = $"Body Fade 3"
#	transparent_bodies = [body0, body1, body2, body3]
	
	body0 = $"Body Fade 0"
	body1 = $"Body Fade 1"
	body2 = $"Body Fade 2"
	body3 = $"Body Fade 3"
	transparent_bodies = [body0, body1, body2, body3]

	transparency_interval_timer = .2
	transparent_convert = true
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(_delta):
#	if Input.is_action_just_pressed("interact"):
#		if transparent_convert:
#			disable_bodies()
#			transparent_convert = false
#		else:
#			enable_bodies()
#			transparent_convert = true
	pass

func disable_bodies():
	for i in range(transparent_bodies.size()):
		transparent_bodies[i].visible = false
		await get_tree().create_timer(transparency_interval_timer).timeout

func enable_bodies():
	for i in range(transparent_bodies.size()):
		transparent_bodies[i].visible = true
		await get_tree().create_timer(transparency_interval_timer).timeout
	
func _on_maiden_ai_manager_turn_transparent():
	if transparent_convert:
		disable_bodies()
		transparent_convert = false
	else:
		enable_bodies()
		transparent_convert = true
