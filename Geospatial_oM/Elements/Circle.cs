using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Geospatial
{
    [Description("Class for representing a geospatial circle.")]
    public class Circle : IGeospatial, IGeospatialRegion
    {
        [Description("Geospatial centre point of the circle.")]
        public virtual Point Centre { get; set; } = new Point();

        [Description("Geospatial radius of the circle.")]
        public virtual double Radius { get; set; } = 0;
    }
}
