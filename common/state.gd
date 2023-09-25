# Based on: https://www.gdquest.com/tutorial/godot/design-patterns/finite-state-machine/

class_name State
extends Node

# Reference to state machine to call `transition_to()`
var state_machine = null

# Receives events from `_unhandled_input()` callback
func handle_input(_event: InputEvent) -> void:
	pass

# Corresponds to the `_process()` callback
func update(_delta: float) -> void:
	pass

# Corresponds to the `_physics_process()` callback
func physics_update(_delta: float) -> void:
	pass

# Called by the state machine upon changing the active state
# `msg` parameter is a dictionary with data the state can use to initialize itself
func enter(_msg := {}) -> void:
	pass

# Called by the state machine before changing to a new state
# Use this function to clean up the state
func exit() -> void:
	pass
