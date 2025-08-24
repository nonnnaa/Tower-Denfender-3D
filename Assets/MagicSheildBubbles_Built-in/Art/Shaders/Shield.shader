// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "StarryMomo/Shield"
{
	Properties
	{
		[HDR]_TintColor("TintColor", Color) = (1,1,1,0)
		[HDR]_ColorUp("ColorUp", Color) = (1,1,1,0)
		[HDR]_ColorDown("ColorDown", Color) = (1,1,1,0)
		_MainTex("MainTex", 2D) = "white" {}
		_MainSpeed("MainSpeed", Vector) = (0,0,0,0)
		_WaveTex("WaveTex", 2D) = "white" {}
		_WaveSpeed("WaveSpeed", Vector) = (0,0,0,0)
		_WaveValue("WaveValue", Float) = 0
		_VertexOffset("VertexOffset", Float) = 0
		_VertexDirection("VertexDirection", Vector) = (0,0,0,0)
		_MaskTex("MaskTex", 2D) = "white" {}
		_MaskColor("MaskColor", Color) = (0,0,0,0)
		_MaskSpeed("MaskSpeed", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		ZTest LEqual
		Blend SrcAlpha One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _VertexOffset;
		uniform float3 _VertexDirection;
		uniform float4 _ColorDown;
		uniform float4 _ColorUp;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float2 _MainSpeed;
		uniform sampler2D _WaveTex;
		uniform float4 _WaveTex_ST;
		uniform float2 _WaveSpeed;
		uniform float _WaveValue;
		uniform float4 _TintColor;
		uniform sampler2D _MaskTex;
		uniform float4 _MaskTex_ST;
		uniform float2 _MaskSpeed;
		uniform float4 _MaskColor;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( sin( ( _VertexOffset * _Time.y ) ) * ( _VertexDirection * ase_vertexNormal ) );
			v.vertex.w = 1;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 lerpResult102 = lerp( _ColorDown , _ColorUp , i.uv_texcoord.y);
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 break12_g14 = _MainSpeed;
			float2 appendResult4_g14 = (float2(( break12_g14.x * _Time.y ) , ( break12_g14.y * _Time.y )));
			float2 uv_WaveTex = i.uv_texcoord * _WaveTex_ST.xy + _WaveTex_ST.zw;
			float2 break12_g12 = _WaveSpeed;
			float2 appendResult4_g12 = (float2(( break12_g12.x * _Time.y ) , ( break12_g12.y * _Time.y )));
			float2 uv_MaskTex = i.uv_texcoord * _MaskTex_ST.xy + _MaskTex_ST.zw;
			float2 break12_g13 = _MaskSpeed;
			float2 appendResult4_g13 = (float2(( break12_g13.x * _Time.y ) , ( break12_g13.y * _Time.y )));
			o.Emission = ( ( lerpResult102 * ( tex2D( _MainTex, ( float4( ( uv_MainTex + appendResult4_g14 ), 0.0 , 0.0 ) + ( tex2D( _WaveTex, ( uv_WaveTex + appendResult4_g12 ) ) * _WaveValue ) ).rg ) * ( i.vertexColor * _TintColor ) ) ) * ( tex2D( _MaskTex, ( uv_MaskTex + appendResult4_g13 ) ) + _MaskColor ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.SamplerNode;75;871.6964,649.1653;Inherit;True;Property;_WaveTex;WaveTex;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;37;1377.298,245.2243;Inherit;True;Property;_MainTex;MainTex;3;0;Create;True;0;0;0;False;0;False;-1;None;dec2322f0bce22c41955babf59b946cc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;1214.765,716.4754;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;1192.402,439.1703;Inherit;False;2;2;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;79;949.5946,892.6609;Inherit;False;Property;_WaveValue;WaveValue;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;1870.309,488.0588;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;100;1907.512,-446.2364;Inherit;False;Property;_ColorUp;ColorUp;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;0.6307573,0.921582,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;101;1910.393,-244.5527;Inherit;False;Property;_ColorDown;ColorDown;2;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;0,0.3277781,0.9734455,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;72;375.5797,377.2874;Inherit;False;0;37;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;103;1911.258,-6.251957;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;102;2326.687,-116.2297;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;2240.156,1948.656;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;1952.573,1896.248;Inherit;False;Property;_VertexOffset;VertexOffset;8;0;Create;True;0;0;0;False;0;False;0;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;85;1938.315,2037.126;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;87;2427.507,1955.594;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;88;1948.72,2173.306;Inherit;False;Property;_VertexDirection;VertexDirection;9;0;Create;True;0;0;0;False;0;False;0,0,0;0.1,0.1,0.1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;2271.464,2225.242;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalVertexDataNode;90;1959.849,2360.646;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;2592.353,2104.676;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;65;3349.871,621.8664;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;StarryMomo/Shield;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;2;False;;3;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;8;5;False;;1;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;_CutMode;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;2421.545,260.1311;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;109;2174.798,706.478;Inherit;True;Property;_MaskTex;MaskTex;10;0;Create;True;0;0;0;False;0;False;-1;None;74c45d2be1c558b43a08d604776118e8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;110;2258.322,933.9958;Inherit;False;Property;_MaskColor;MaskColor;11;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.4433962,0.4433962,0.4433962,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;113;2891.945,746.4088;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;97;1528.664,515.6242;Inherit;False;Property;_TintColor;TintColor;0;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;2,2,2,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;114;1562.613,699.3585;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;1760.613,701.3585;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;112;2645.393,814.557;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;78;330.7771,790.1129;Inherit;False;0;75;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;77;346.9959,651.1562;Inherit;False;Property;_WaveSpeed;WaveSpeed;6;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;117;1609.409,1027.667;Inherit;False;0;109;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;118;1625.628,888.7101;Inherit;False;Property;_MaskSpeed;MaskSpeed;12;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;68;425.4221,197.5855;Inherit;False;Property;_MainSpeed;MainSpeed;4;0;Create;True;0;0;0;False;0;False;0,0;0.2,10;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;125;582.1613,681.2211;Inherit;False;Tiling;-1;;12;d809bb332af44a243bc38dc78d4c59c2;0;2;11;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;126;1860.793,918.7751;Inherit;False;Tiling;-1;;13;d809bb332af44a243bc38dc78d4c59c2;0;2;11;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;127;702.5189,245.4603;Inherit;False;Tiling;-1;;14;d809bb332af44a243bc38dc78d4c59c2;0;2;11;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
WireConnection;75;1;125;0
WireConnection;37;1;82;0
WireConnection;80;0;75;0
WireConnection;80;1;79;0
WireConnection;82;0;127;0
WireConnection;82;1;80;0
WireConnection;98;0;37;0
WireConnection;98;1;115;0
WireConnection;102;0;101;0
WireConnection;102;1;100;0
WireConnection;102;2;103;2
WireConnection;86;0;83;0
WireConnection;86;1;85;0
WireConnection;87;0;86;0
WireConnection;89;0;88;0
WireConnection;89;1;90;0
WireConnection;91;0;87;0
WireConnection;91;1;89;0
WireConnection;65;2;113;0
WireConnection;65;11;91;0
WireConnection;106;0;102;0
WireConnection;106;1;98;0
WireConnection;109;1;126;0
WireConnection;113;0;106;0
WireConnection;113;1;112;0
WireConnection;115;0;114;0
WireConnection;115;1;97;0
WireConnection;112;0;109;0
WireConnection;112;1;110;0
WireConnection;125;11;77;0
WireConnection;125;8;78;0
WireConnection;126;11;118;0
WireConnection;126;8;117;0
WireConnection;127;11;68;0
WireConnection;127;8;72;0
ASEEND*/
//CHKSM=A5A48F7EBEE6E74FE72C5CC94F4749A7EB1CAA1E