extends Node3D


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("sword special"):
		print("Ice Ability Used")
#Give player velocity upwards
#after reaching certain height from starting point slam downwards
#Enemy touched by this blade will freeze on special
#attacking enemy with basic attacks has a chance of freezing enemy
