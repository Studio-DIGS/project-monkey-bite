class_name InventoryVis
extends Node

@onready var swordTextLabel = $UICanvasGroup/SwordText
@onready var bigPassiveTextLabel = $UICanvasGroup/BigPassiveText
@onready var angelAbilityTextLabel = $UICanvasGroup/AngelAbilityText
@onready var health_bar = $UICanvasGroup/HealthBar
@onready var health_number = $UICanvasGroup/_101
@onready var weapon_sprite = $UICanvasGroup/WeaponHud/WeaponSprite
@onready var fist_sprite: CompressedTexture2D = preload("res://assets/UI/fist.png")
@onready var face: AnimatedSprite2D = $UICanvasGroup/AnimatedSprite2D
var health_percent: float = 100

func _ready():
#	swordTextLabel.clear()
#	bigPassiveTextLabel.clear()
#	angelAbilityTextLabel.clear()
#	updateAllText()
	pass

func _process(delta):
	health_bar.value = lerp(health_bar.value, health_percent, 10 * delta)

func update_health(current_hp: int, max_hp: int):
	health_percent = (float(current_hp) / float(max_hp)) * 100
	health_number.text = str(current_hp)

func update_weapon(sprite: CompressedTexture2D = fist_sprite):
	weapon_sprite.texture = sprite

func update_face(frame: int):
	face.frame = frame

func updateAllText():
	updateAngelAbilityVis()
	updateBigPassiveVis()
	updateSwordVis()

# Sword

func updateSwordVis():
	swordTextLabel.clear()
	swordTextLabel.append_text(InventoryManager.getSword().name)

# Big Passives
	
func updateBigPassiveVis():
	if (InventoryManager.getBigPassives().size() <= 0):
		return
	bigPassiveTextLabel.clear()
	for bigPassive in InventoryManager.getBigPassives():
		bigPassiveTextLabel.append_text(bigPassive.name)

# Angel Abilites
	
func updateAngelAbilityVis():
	angelAbilityTextLabel.clear()
	angelAbilityTextLabel.append_text(InventoryManager.getAngelAbility().name)
