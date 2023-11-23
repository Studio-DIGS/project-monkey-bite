extends CameraProcesser
class_name PixelCamBase

@export_group("Dependencies")
@export var viewport: SubViewport
@export var display_rect : ColorRect

var pixel_size : float
		
func process_cam(_dir : float, current_position : Vector3, _delta : float, camera : Camera3D) -> Vector3:
	# Update pixel size, if it's changed
	pixel_size = calculate_pixel_size(camera, viewport)
	if(display_rect != null):
		display_rect.material.set_shader_parameter("game_resolution", viewport.size)
		
	# apply snap
	var snap_result = get_pixel_snapped_pos(current_position, camera.basis)
	update_display(snap_result)
	return snap_result.snapped_world_pos
	
func calculate_pixel_size(camera : Camera3D, viewport : SubViewport) -> float:
	return camera.size / viewport.size.y
	
class SnapResult:
	var snapped_world_pos : Vector3
	
	# 0 to 1, pixel snap offset in uv space
	var snap_pixel_offset_uv : Vector2
	
	func _init(pos : Vector3, snap_pixel_offset_uv : Vector2):
		self.snapped_world_pos = pos
		self.snap_pixel_offset_uv = snap_pixel_offset_uv
	

func get_pixel_snapped_pos(world_pos : Vector3, cam_basis : Basis) -> SnapResult:
	# Create rotation space matrix
	var rot_space_transform = Transform3D(cam_basis)
	var rot_space_transform_inv = rot_space_transform.inverse()
	
	# transform pos to rotation space
	var local_pos = rot_space_transform_inv * world_pos
	
	# snap
	var snapped_x = int(local_pos.x / pixel_size) * pixel_size
	var snapped_y = int(local_pos.y / pixel_size) * pixel_size
	
	# calculate offset for use in sub-pixel camera
	var snap_pixel_offset_uv = Vector2(snapped_x - local_pos.x, snapped_y - local_pos.y) / pixel_size
	
	snap_pixel_offset_uv.x /= viewport.size.x
	# negate y since UV has y increasing downwards
	snap_pixel_offset_uv.y /= -viewport.size.y
	
	var snapped_local_pos = Vector3(
		snapped_x,
		snapped_y,
		local_pos.z
		)
		
	# transform back to world space
	var final_pos = rot_space_transform * snapped_local_pos
	
	var result = SnapResult.new(final_pos, snap_pixel_offset_uv)
	return result
	
func update_display(snap_result : SnapResult):
	# update the display rect to match snap offset
	display_rect.material.set_shader_parameter("pixel_snap_uv_offset", snap_result.snap_pixel_offset_uv)
