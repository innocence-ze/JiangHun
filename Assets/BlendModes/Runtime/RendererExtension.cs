// Copyright 2014-2018 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UnityEngine;

namespace BlendModes
{
    public abstract class RendererExtension<TComponent> : ComponentExtension where TComponent : Renderer
    {
        protected string DefaultShaderName { get { return GetDefaultShaderName(); } }

        private static Material defaultMaterial;

        public override void OnEffectEnabled ()
        {
            if (!defaultMaterial) defaultMaterial = new Material(Shader.Find(DefaultShaderName));
        }

        public override void OnEffectDisabled ()
        {
            if (!IsExtendedComponentValid) return;
            GetExtendedComponent<TComponent>().sharedMaterials = new [] { defaultMaterial };
        }

        public override Material[] GetRenderMaterials ()
        {
            return GetExtendedComponent<TComponent>().sharedMaterials;
        }

        public override void SetRenderMaterials (Material[] materials)
        {
            GetExtendedComponent<TComponent>().sharedMaterials = materials;
        }

        protected abstract string GetDefaultShaderName ();
    }
}
