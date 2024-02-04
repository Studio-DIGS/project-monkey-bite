class_name AttackCommand
extends Command

func execute(actor: Actor, _data: Object = null) -> void:
	actor.attack()
