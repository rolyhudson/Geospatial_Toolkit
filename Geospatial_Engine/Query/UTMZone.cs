/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using BH.oM.Geospatial;
using BH.oM.Base.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Geospatial
{
    public static partial class Query
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        [Description("Method for querying an IGeospatial object for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int IUTMZone(IGeospatial geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return -1;
            }
            return UTMZone(geospatial as dynamic);
        }

        /***************************************************/

        [Description("Convert longitude to Universal Transverse Mercator zone.")]
        [Input("longitude", "The longitude to convert, in the range -180.0 to 180.0 with up to 7 decimal places.")]
        [Output("utmZone", "Universal transverse Mercator zone.")]
        public static int UTMZone(this double longitude)
        {
            return (int)Math.Floor((longitude + 180) / 6) + 1;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial object for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this Point geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return -1;
            }
            return UTMZone(geospatial.Longitude);
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial object for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this BoundingBox geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return -1;
            }
            return (int)((UTMZone(geospatial.Min) + UTMZone(geospatial.Max)) / 2.0);
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial MultiPoint for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this MultiPoint geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return -1;
            }
            int zone = 0;
            foreach(Point p in geospatial.Points)
                zone += UTMZone(p.Longitude);
            return (int)zone / geospatial.Points.Count;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial LineString for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this LineString geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return -1;
            }
            int zone = 0;
            foreach (Point p in geospatial.Points)
                zone += UTMZone(p);
            return (int)zone / geospatial.Points.Count;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial MultiLineString for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this MultiLineString geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return -1;
            }
            int zone = 0;
            foreach (LineString lineString in geospatial.LineStrings)
                zone += UTMZone(lineString);
            return (int)zone / geospatial.LineStrings.Count;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial Polygon for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this Polygon geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return -1;
            }
            int zone = 0;
            foreach (LineString lineString in geospatial.Polygons)
                zone += UTMZone(lineString);
            return (int)zone / geospatial.Polygons.Count;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial MultiPolygon for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this MultiPolygon geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return -1;
            }
            int zone = 0;
            foreach (Polygon polygon in geospatial.Polygons)
                zone += UTMZone(polygon);
            return (int)zone / geospatial.Polygons.Count;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial Feature for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this Feature geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return -1;
            }
            return UTMZone(geospatial.Geometry as dynamic); 
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial FeatureCollection for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this FeatureCollection geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return -1;
            }
            int zone = 0;
            foreach (Feature feature in geospatial.Features)
                zone += UTMZone(feature.Geometry as dynamic);
            return (int)zone / geospatial.Features.Count;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial GeometryCollection for its zone in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("zone", "The UTM zone. If the input IGeospatial includes objects that span multiple zones the average zone is returned.")]
        public static int UTMZone(this GeometryCollection geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return -1;
            }
            int zone = 0;
            foreach (IGeospatial feature in geospatial.Geometries)
                zone += UTMZone(feature as dynamic);
            return (int)zone / geospatial.Geometries.Count;
        }

        /***************************************************/
        /****           Private Fallback Method         ****/
        /***************************************************/
        private static int UTMZone(IGeospatial geospatial)
        {
            Base.Compute.RecordWarning("UTM zone could not be found.");
            return 0;
        }
    }
}

