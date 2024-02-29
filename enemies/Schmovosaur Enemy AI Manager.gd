extends Node
#Notes: Consider playing the particles as a one shot instead of continuously for optimization
#Consider rotating enemy instead of changing their walking position

var isPassiveState: bool #When player is not around or seen
var isActiveState: bool #When player is within enemy sight sphere
var isChargeReady: bool

var tetherDistance: float

var enemyBody: Node3D 
var enemySpawnPoint: Node3D
var enemyDirection
var playerBody
var playerDirection: Vector3
var playerChargeDirection: Vector3

var horizontalInput
var enemySpeed
var enemySpeedOrigin: int = 1

var chargeCooldownTimer
var prepareChargeTimer
var chargeDurationTimer

var chargeParticles
var chargePreparationParticles
var rotationOrigin
# Called when the node enters the scene tree for the first time.
func _ready():
	isPassiveState = true
	isActiveState = false
	
	enemySpeed = enemySpeedOrigin
	enemyBody = $"../EnemyComponents" #Change this if you are messing around with Enemy AI Manager's location for children
	enemySpawnPoint = $"../SpawnPointMarker"
	
	chargeCooldownTimer = $"Charge Cooldown"
	prepareChargeTimer = $"Prepare Charge"
	chargeDurationTimer = $"Charge Duration"
	
	chargeParticles = $"../EnemyComponents/Charge Particles"
	chargePreparationParticles = $"../EnemyComponents/Charge Preparation Particles"
	chargeParticles.visible = true
	chargePreparationParticles.visible = true
	chargeParticles.emitting = false
	chargePreparationParticles.emitting = false
	rotationOrigin = 0
	
	horizontalInput = 0
	tetherDistance = 5
	
	isChargeReady = true


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	horizontalInput = Input.get_axis("TempMoveLeft", "TempMoveRight")
	if isPassiveState == true: #Turn mob around if too far from spawnPoint
		if enemyBody.position.x - enemySpawnPoint.position.x > tetherDistance:
			enemySpeed = -enemySpeedOrigin
		elif enemyBody.position.x - enemySpawnPoint.position.x < -tetherDistance:
			enemySpeed = enemySpeedOrigin
		if horizontalInput != 0:
			enemyBody.position += Vector3(1, 0, 0) * horizontalInput * enemySpeed * delta
		else:
			enemyBody.position.x += enemySpeed * delta
		
	
	
	if isActiveState == true:
		if Input.is_action_just_pressed("interact") or isChargeReady:
			chargeState()
		playerDirection = (playerBody.position - enemyBody.position).normalized()
		enemyBody.position.x += enemySpeed * delta * (playerDirection.x + playerChargeDirection.x)
	
	

func dealDamage():
	print("Player was hit")

#States
func passiveState(): #Enemy wanders around
	print("Passive State")
	isPassiveState = true
	isActiveState = false

func activeState():
	print("Active State") #Enemy is walking towards player
	isPassiveState = false
	isActiveState = true
	enemySpeed = enemySpeedOrigin
	
func rotateParticles():
	if enemyBody.position.x < playerBody.position.x:
		chargeParticles.rotate(Vector3(0,1,0), 0)
		chargePreparationParticles.rotate(Vector3(0,1,0), 0)
		print("Rotates 0")
	else:
		chargeParticles.rotate(Vector3(0,1,0), PI)
		chargePreparationParticles.rotate(Vector3(0,1,0), PI)
		print("Rotates 180")

func chargeState(): #Enemy is attack
	isChargeReady = false
	print("Charge State")
	prepareChargeTimer.start()
	chargePreparationParticles.emitting = true
	enemySpeed = 0
	playerChargeDirection = (playerBody.position - enemyBody.position).normalized()
	rotateParticles()

func _on_prepare_charge_timeout():
	#Make enemy movement speed faster
	enemySpeed = enemySpeedOrigin * 10
	chargeDurationTimer.start()
	chargeParticles.emitting = true
	print("Charge ended")
	rotateParticles()

func _on_charge_duration_timeout():
	chargeCooldownTimer.wait_time = 3 + randf_range(-1, 2)
	enemySpeed = enemySpeedOrigin
	playerChargeDirection.x = 0
	print(chargeCooldownTimer.wait_time)
	chargeCooldownTimer.start() #Enemy cannot charge 

func _on_charge_cooldown_timeout():
	isChargeReady = true
	
#Checks if player is within "PlayerDetectionSphere"
func _on_player_detection_sphere_area_entered(area):
	if area.name == "PlayerArea":
		activeState()
		playerBody = area.get_parent()

func _on_player_detection_sphere_area_exited(area):
	if area.name == "PlayerArea":
		passiveState()

