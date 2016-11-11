///<summary>
/// viewed: https://gist.github.com/bbeaumont/9537b52506fa171d062c
/// https://bitbucket.org/Unity-Technologies/ui/src/0155c39e05ca5d7dcc97d9974256ef83bc122586/UnityEngine.UI/UI/Core/VertexModifiers/Outline.cs?at=5.2&fileviewer=file-view-default
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

        /*protected new void ApplyShadowZeroAlloc(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
        {
            UIVertex vt;

            var neededCpacity = verts.Count * 2;
            if (verts.Capacity < neededCpacity)
                verts.Capacity = neededCpacity;

            for (int i = start; i < end; ++i)
            {
                vt = verts[i];
                verts.Add(vt);

                Vector3 v = vt.position;
                v.x += x;
                v.y += y;
                vt.position = v;
                vt.color = Color.white;// color;
                verts[i] = vt;
            }
        }*/

        public override void ModifyMesh(VertexHelper vertexHelper)
        {
            //effectColor = Color.white;
            //base.ModifyMesh(vertexHelper);

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