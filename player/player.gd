class_name Player
extends Actor

@onready var anim = $AnimationPlayer
@export var speed = 5.0
@export var accel = 15.0
@export var jump_height = 4.5
# Get the gravity from the project settings to be synced with RigidBody nodes.
#var gravity = ProjectSettings.get_setting("physics/3d/default_gravity")
var gravity = 20

# direction player is facing (1 is forward, -1 is backwards)
var orientation = 1
signal turn_around

var is_attack = false
@onready var controller_container = $ControllerContainer

# Attack stuff
@export var hitbox: Hitbox
@export var combo: Array[AttackResource] = []
@export var air_combo: Array[AttackResource] = []
@export var zone_in: AttackResource
@onready var zone_in_dist: RayCast3D = $Mesh/ZoneInDist
@onready var stay_put_dist: RayCast3D = $Mesh/StayPutDist


func _ready():
	set_controller()
	GameManager.connect("end_cutscene", set_controller)

func set_controller(controller: Controller = null):
	# delete all previous controllers
	for child in controller_container.get_children():
		child.queue_free()
	
	if controller == null:
		controller = HumanController.new(self)
	controller_container.add_child(controller)

func attack():
	is_attack = true

func _physics_process(_delta):
	orient()

func orient():
	# stores current orientation
	var new_orientation = orientation
	if hori_input > 0:
		new_orientation = 1
	elif hori_input < 0:
		new_orientation = -1

	# Turn player around
	if new_orientation != orientation:
		orientation = new_orientation
		emit_signal("turn_around")
