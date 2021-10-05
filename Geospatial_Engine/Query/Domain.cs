using BH.oM.Data.Collections;
using BH.oM.Geospatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Geospatial
{
    public static partial class Query
    {
        public static Domain Domain(this BoundingBox boundingBox, string axis)
        {
            switch (axis)
            {
                case "Latitude":
                    return new Domain(boundingBox.Min.Latitude, boundingBox.Max.Latitude);
                case "Longitude":
                    return new Domain(boundingBox.Min.Longitude, boundingBox.Max.Longitude);
                case "Altitude":
                    return new Domain(boundingBox.Min.Altitude, boundingBox.Max.Altitude);
                default:
                    Reflection.Compute.RecordError($"Axis {axis} is not one of the axes associated with the Geospatial.BoundingBox only Latitude, Longitude or Altitude are permitted axes.");
                    return null;
            }
        }
    }
}
