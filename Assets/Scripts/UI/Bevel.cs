///<summary>
/// viewed: https://gist.github.com/bbeaumont/9537b52506fa171d062c
///</summary>

using System.Collections.Generic;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Effects/Bevel", 15)]
    public class Bevel : Shadow//, IMeshModifier
    {
        [SerializeField]
        private Color highlightColor;

        protected Bevel()
        { }

        //public override void ModifyVertices(List<UIVertex> verts)
        public override void ModifyMesh(Mesh a_mesh)
        {
            List<UIVertex> vertexList = new List<UIVertex>();
            using (VertexHelper vertexHelper = new VertexHelper(a_mesh))
            {
                if (!IsActive())
                    return;

                var start = 0;
                var end = vertexHelper.currentVertCount;
                ApplyShadow(vertexList, highlightColor, start, vertexHelper.currentVertCount, -effectDistance.x, effectDistance.y);

                start = end;
                ApplyShadow(vertexList, effectColor, start, vertexHelper.currentVertCount, effectDistance.x, -effectDistance.y);
            }
        }
    }
}