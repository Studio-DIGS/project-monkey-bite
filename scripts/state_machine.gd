# Based on: https://www.gdquest.com/tutorial/godot/design-patterns/finite-state-machine/

class_name StateMachine
extends Node

signal transitioned(state_name)
@export var initial_state := NodePath()

# Current active state
@onready var state: State = get_node(initial_state)

# Called when the node enters the scene tree for the first time.
func _ready():
	# wait for parent?
	for child in get_children():
		child.state_machine = self
	state.enter()

# Sends unhandled input events to the state
# See `_unhandled_input()` in the docs for more info
func _unhandled_input(event):
	state.handle_input(event)

func _process(delta):
	state.update(delta)

func _physics_process(delta):
	state.physics_update(delta)

func transition_to(target_state_name: String, msg: Dictionary = {}):
	if not has_node(target_state_name):
		return
	
	state.exit()
	state.get_node(target_state_name)
	state.enter(msg)
	emit_signal("transitioned", state.name)
