extends Node

@export var light_gradient:GradientTexture1D
@export var sky_gradient:GradientTexture1D

var time = 0
@export var time_dilation = 5
@export var world_env:WorldEnvironment;

func _process(delta):
	
	time += delta
	var x = (sin((time - PI / 2) / time_dilation) + 1) / 2
	self.light_color = light_gradient.gradient.sample(x)
	world_env.environment.sky.sky_material.set_shader_parameter("sky_top_color", sky_gradient.gradient.sample(x))
	world_env.environment.sky.sky_material.set_shader_parameter("sky_horizon_color", sky_gradient.gradient.sample(x))
	world_env.environment.sky.sky_material.set_shader_parameter("ground_bottom_color", sky_gradient.gradient.sample(x))
	world_env.environment.sky.sky_material.set_shader_parameter("ground_horizon_color", sky_gradient.gradient.sample(x))
