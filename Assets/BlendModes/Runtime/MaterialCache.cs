// Copyright 2014-2018 Elringus (Artyom Sovetnikov). All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace BlendModes
{
    /// <summary>
    /// Manages cached materials used by <see cref="BlendModeEffect"/>.
    /// </summary>
    public static class MaterialCache 
    {
        private static List<Material> cachedMaterials = new List<Material>();

        /// <summary>
        /// Based on the current requirements of the provided <see cref="BlendModeEffect"/> object
        /// and shader, retrieves a cached or creates new material and adds it to the cache.
        /// </summary>
        public static Material GetSharedFor (Shader shader, BlendModeEffect blendModeEffect, out bool materialCreated)
        {
            materialCreated = false;
            var blendModeKeyword = blendModeEffect.BlendMode.ToShaderKeyword();
            for (int i = 0; i < cachedMaterials.Count; i++)
            {
                var material = cachedMaterials[i];
                if (!material) continue;
                if (material.shader == shader && material.IsKeywordEnabled(blendModeKeyword))
                    return material;
            }
            materialCreated = true;
            return CreateCachedMaterial(shader);
        }

        /// <summary>
        /// Destroyes all the currently created materials.
        /// </summary>
        public static void DestroyCachedMaterials ()
        {
            for (int i = 0; i < cachedMaterials.Count; i++)
            {
                var material = cachedMaterials[i];
                if (!material) continue;
                if (Application.isPlaying) Object.Destroy(material);
                else Object.DestroyImmediate(material);
            }
            cachedMaterials.Clear();
        }

        private static Material CreateCachedMaterial (Shader shader)
        {
            var material = new Material(shader);
            material.hideFlags = HideFlags.HideAndDontSave;
            cachedMaterials.Add(material);
            return material;
        }
    }
}
