    Shader "Toon/Reflect Outline" {
        Properties {
            _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
            _OutlineColor ("Outline Color", Color) = (0,0,0,1)
            _Outline ("Outline width", Range (.002, 0.03)) = .005
            _MainTex ("Base (RGB)", 2D) = "white" {}
            _SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
            _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
            _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
            _Cube ("Reflection Cubemap", Cube) = "" { TexGen CubeReflect }
            _BumpMap ("Normalmap", 2D) = "bump" {}
           
        }
     
        SubShader {
            Tags { "RenderType"="Opaque" }
            UsePass "Reflective/Bumped Specular/FORWARD"
            UsePass "Toon/Basic Outline/OUTLINE"
        }
       
        Fallback "Toon/Lighted"
    }
