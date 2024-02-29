shader_type canvas_item;

// Viewport texture to render to screen
uniform sampler2D viewport_tex : filter_nearest;

// 0 to 1, the offset amount used to snap the camera to a pixel grid
// We'll be "undoing" the pixel snap in post process to get a smooth camera
uniform vec2 texel_snap_uv_offset;

void fragment() 
{
	// Pixel snap undo
	vec2 uv = UV;
	uv -= texel_snap_uv_offset;
	
	// Ramp texture
	vec4 color = texture(viewport_tex, uv);
	
	COLOR = color;
}
