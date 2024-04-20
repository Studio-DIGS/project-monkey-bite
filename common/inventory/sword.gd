extends Resource
class_name Sword

@export var name : String
@export var damage = 10.0
@export var knockback = 5.0
@export var mesh: PackedScene
@export var sprite: CompressedTexture2D
@export var special_attack: AttackResource
@export var special_scene: PackedScene
@export var throw_speed = 15.0
@export var flight_time = 0.5 # time sword will fly straight when thrown
