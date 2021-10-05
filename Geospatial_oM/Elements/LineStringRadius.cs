using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Geospatial
{
    [Description("Class for representing a GeoJSON LineString with thickness.")]
    public class LineStringRadius : IGeospatial, IGeospatialRegion
    {
        [Description("GeoJSON LineString.")]
        public virtual LineString LineString { get; set; } = new LineString();

        [Description("Distance around the LineString.")]
        public virtual double Radius { get; set; } = 0;
    }
}
