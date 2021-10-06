using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Geospatial
{
    [Description("Class for representing a geospatial bounding box in The World Geodetic System (WGS 1984, EPSG:4326).")]
    public class BoundingBox : IGeospatial, IGeospatialRegion
    {
        [Description("The lower bound values for the Longitude, Latitude and Altitude coordinates of the Box corner Points.")]
        public virtual Point Min { get; set; } = new Point();

        [Description("The upper bound values for the Longitude, Latitude and Altitude coordinates of the Box corner Points.")]
        public virtual Point Max { get; set; } = new Point();

        /***************************************************/
        /**** Static Operators Override                 ****/
        /***************************************************/

        public static BoundingBox operator +(BoundingBox a, BoundingBox b)
        {
            if (a == null || b == null)
                return null;

            return new BoundingBox
            {
                Min = new Point
                {
                    Latitude = Math.Min(a.Min.Latitude, b.Min.Latitude),
                    Longitude = Math.Min(a.Min.Longitude, b.Min.Longitude),
                    Altitude = Math.Min(a.Min.Altitude, b.Min.Altitude)
                },
                Max = new Point
                {
                    Latitude = Math.Max(a.Max.Latitude, b.Max.Latitude),
                    Longitude = Math.Max(a.Max.Longitude, b.Max.Longitude),
                    Altitude = Math.Max(a.Max.Altitude, b.Max.Altitude)
                }
            };
        }
    }

}
