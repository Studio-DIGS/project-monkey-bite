class_name Player
extends Actor

@onready var anim = $pmb_kite/AnimationPlayer
@export var speed = 5.0
@export var accel = 15.0

@export var min_jump_height = 2.0
@export var max_jump_height = 4.5
var jump_time = 0.0
@export var jump_force_coefficient = 10.0
var gravity = 20

# direction player is facing (1 is forward, -1 is backwards)
var orientation = 1
signal turn_around

# Commands for state machine
var try_attack = false
var try_jump = false
var try_throw = false
var try_interact = false
@onready var controller_container = $ControllerContainer

# Attack stuff
@export var hitbox: Hitbox
@export var combo: Array[AttackResource] = []
@export var air_combo: Array[AttackResource] = []
@export var zone_in: AttackResource
#@onready var zone_in_dist: RayCast3D = $Mesh/ZoneInDist
#@onready var stay_put_dist: RayCast3D = $Mesh/StayPutDist
@export var punch: Array[AttackResource] = []
@export var air_kick: Array[AttackResource] = []
var is_armed = true


@onready var controllers = $ControllerContainer
@onready var human_controller = $ControllerContainer/HumanController
@onready var cutscene_controller = $ControllerContainer/CutsceneController

@onready var sword_body = preload("res://player/swords/sword_body.tscn")
@onready var sword_spawn = $pmb_kite/base_human_rig/Skeleton3D/BoneAttachment3D
@onready var sword_holder = $pmb_kite/base_human_rig/Skeleton3D/BoneAttachment3D/SwordHolder

@onready var interaction_area: Area3D = $PlayerInteraction
@export var inventoryVis: InventoryVis

func _ready():
	print(GameManager.current_scene.name)
	if(InventoryManager.uninitialized == true):
		InventoryManager.swapSword("Amazing Sword")
		InventoryManager.addBigPassive(BigPassive.new("Huge Passive"))
		InventoryManager.setAngelAbility(AngelAbility.new("God Hacks"))
		InventoryManager.uninitialized = false
	set_controller(human_controller)
	GameManager.connect("start_cutscene", _start_cutscene)
	GameManager.connect("end_cutscene", _end_cutscene)
	

func updateInventory():
	if inventoryVis:
		inventoryVis.updateAllText()

func set_controller(controller: PlayerController):
	# turn off all other controllers
	for child in controllers.get_children():
		if child is PlayerController:
			child.is_active = false
	
	hori_input = 0.0
	stop_jump()
	controller.is_active = true

func _start_cutscene(_cutscene):
	set_controller(cutscene_controller)

func _end_cutscene():
	set_controller(human_controller)

func attack():
	try_attack = true
	await get_tree().process_frame
#	await get_tree().process_frame
	try_attack = false

func throw():
	try_throw = true
	await get_tree().process_frame
	try_throw = false;

#func stop_attack():
#	try_attack = false

func jump(delta):
	try_jump = true
	jump_time += delta

func stop_jump():
	try_jump = false
	jump_time = 0.0

func drop_platform():
	set_collision_mask_value(9, false)
	
func interact():
	try_interact = true
	print("trying to interact")
	await get_tree().create_timer(0.1).timeout
	interaction_area.set_deferred("monitorable", false)
	interaction_area.set_deferred("monitoring", false)
	try_interact = false
	print("interaction over")

func _physics_process(_delta):
	if get_collision_mask_value(9) and vert_input < 0.0:
		drop_platform()

	velocity.z = 0.0

func reorient():
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


func _on_player_interaction_swap_swords(sword):
	if not is_armed:
		var new_sword = sword.stats.mesh.instantiate()
#		new_sword.transform = sword_holder.transform
		sword_holder.add_child(new_sword)
		
		sword.queue_free() # delete the sword that was on the ground
		is_armed = true
	
