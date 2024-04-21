extends Node3D

@onready var sword = preload("res://player/swords/sword_body.tscn")
@onready var shmovosaur = preload("res://enemies/Schmovosaur/Schmovosaur Enemy.tscn")
@onready var maiden = preload("res://enemies/Maiden/maiden_enemy.tscn")
@onready var spawn_loc = $SpawnLocation

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
#	if Input.is_action_just_pressed("reset"):
#		GameManager.goto_scene("res://levels/grasstest.tscn")
	
	if Input.is_action_just_pressed("spawn-sword"):
		var sword_instance = sword.instantiate()
		sword_instance.rotation = Vector3(0,0,-90)
		self.add_child(sword_instance)
	
	if Input.is_action_just_pressed("spawn-shmovosaur"):
		var shmovo_instance = shmovosaur.instantiate()
		self.add_child(shmovo_instance)
		
	if Input.is_action_just_pressed("spawn-maiden"):
		var maiden_instance = maiden.instantiate()
		self.add_child(maiden_instance)
