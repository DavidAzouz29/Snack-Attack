///<summary>
/// viewed: https://gist.github.com/bbeaumont/9537b52506fa171d062c
/// http://stackoverflow.com/questions/36851992/where-modifyvertices-from-basevertexeffect-moved-in-basemesheffect
///</summary>

using System.Collections.Generic;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Effects/Outline White", 16)]
    public class OutlineWhite : Outline
    {
        public Material c_TextMat;
        [SerializeField]
        private Color highlightColor;

        protected OutlineWhite()
        { }

        public override void ModifyMesh(VertexHelper vertexHelper)
        {
            //Debug.Log("OW: modify");
            List<UIVertex> vertexList = new List<UIVertex>();
            //using (VertexHelper vertexHelper = new VertexHelper(a_mesh))
            //{
                // Move previous VH-related code that you need to keep here
                if (!IsActive())
                    return;

                vertexHelper.GetUIVertexStream(vertexList);

                // ModifyVertices(vertexList);


                for (int i = 0; i < vertexHelper.currentVertCount; i++)
                {
                    UIVertex vert = new UIVertex();
                   // vert.color = Color.white;
                   // vertexList[i] = vert;
                    vertexHelper.PopulateUIVertex(ref vert, i);
                    //vert.uv0 = new Vector2(0.5f, 0.5f);
                    //vert.color = c_TextMat.color;
                    vertexHelper.SetUIVertex(vert, i);
                }


            //vertexHelper.Clear();
            //vertexHelper.AddUIVertexTriangleStream(vertexList);



           // vertexHelper.Dispose();

                //ApplyShadow(vertexList, Color.white, 0, vertexHelper.currentVertCount, effectDistance.x, effectDistance.y);

            /*int count = vertexList.Count;
            float bottomY = vertexList[0].position.y;
            float topY = vertexList[0].position.y;

            for (int i = 1; i < count; i++)
            {
                float y = vertexList[i].position.y;
                if (y > topY)
                {
                    topY = y;
                }
                else if (y < bottomY)
                {
                    bottomY = y;
                }
            }

            float uiElementHeight = topY - bottomY;

            for (int i = 0; i < count; i++)
            {
                UIVertex uiVertex = vertexList[i];
                uiVertex.color = Color.white;// Color32.Lerp(highlightColor, effectColor, (uiVertex.position.y - bottomY) / uiElementHeight);
                vertexList[i] = uiVertex;
            }*/
            // }
        }
    }
}