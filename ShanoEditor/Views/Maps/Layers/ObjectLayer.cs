using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScenarioLib;
using ShanoEditor.ViewModels;
using IO;
using IO.Common;

using Bitmap = System.Drawing.Bitmap;
using Graphics = System.Drawing.Graphics;

namespace ShanoEditor.Views.Maps.Layers
{
    class ObjectLayer : MapLayer, IScenarioControl
    {
        public ScenarioViewModel Model { get; private set; }

        Bitmap objectBitmap;
        MapConfig Map;

        public WorldView View { get; private set; }

        public ObjectLayer(WorldView view) 
        {
            View = view;

            view.ScaleChanged += redrawView;
        }

        double unitSz;
        Point bmpSz;

        void redrawView()
        {
            if (Model == null || Map == null)
                return;

            //should be fine w/ map sizes up to 512x512
            unitSz = Math.Min(View.UnitSize, 10);
            bmpSz = ((Vector)View.MapSize * unitSz).Ceiling();

            objectBitmap = new Bitmap(bmpSz.X, bmpSz.Y);

            using (var g = Graphics.FromImage(objectBitmap))
                foreach (var obj in Map.ObjectList)
                {
                    var go = Model.Scenario.TryGet(obj.TypeName);
                    var anim = Model.Content.ModelDefaultAnimations.TryGet(go?.ModelName);

                    if(go != null && anim != null)
                        anim.Paint(g, new RectangleF(go.Position - go.Scale / 2, new Vector(go.Scale)));
                }
        }

        public void Draw(Graphics g)
        {
            var pos = -(View.LowLeftPoint * unitSz);
            var sz = (Vector)bmpSz / unitSz * View.UnitSize;
            var bounds = new RectangleF(pos, sz);
            if(objectBitmap != null)
                g.DrawImage(objectBitmap, bounds.ToNetRectangle());
        }

        public void Load(MapConfig map)
        {
            Map = map;
            redrawView();
        }

        public void Update()
        {
            redrawView();
        }

        public void SetModel(ScenarioViewModel model)
        {
            Model = model;
            redrawView();
        }
    }
}
