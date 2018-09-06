using UnityEngine;

namespace BlendModes
{
    [ExtendedComponent(typeof(SpriteRenderer))]
    public class SpriteRendererExtension : RendererExtension<SpriteRenderer>
    {
        public override string[] GetSupportedShaderFamilies ()
        {
            return new[] {
                "SpritesDefault"
            };
        }

        protected override string GetDefaultShaderName ()
        {
            return "Sprites/Default";
        }
    }
}
