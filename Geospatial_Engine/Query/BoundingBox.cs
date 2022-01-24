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
 *                                                                              BH.oM.Reflection.Attributes
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

        [Description("Method for querying an IGeospatial object for its Geospatial BoundingBox.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("box", "The BoundingBox.")]
        public static BoundingBox IBoundingBox(IGeospatial geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return null;
            }
            return BoundingBox(geospatial as dynamic);
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial Point for its Geospatial BoundingBox.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("box", "The BoundingBox.")]
        public static BoundingBox BoundingBox(this Point geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return null;
            }
            return new BoundingBox(){ Min = geospatial, Max = geospatial };
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial MultiPoint for its Geospatial BoundingBox.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("box", "The BoundingBox.")]
        public static BoundingBox BoundingBox(this MultiPoint geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return null;
            }
            return BoundingBox(geospatial.Points);
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial LineString for its Geospatial BoundingBox.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("box", "The BoundingBox.")]
        public static BoundingBox BoundingBox(this LineString geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return null;
            }

            return BoundingBox(geospatial.Points);
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial MultiLineString for its Geospatial BoundingBox.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("box", "The BoundingBox.")]
        public static BoundingBox BoundingBox(this MultiLineString geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return null;
            }

            return BoundingBox(geospatial.LineStrings.SelectMany(p => p.Points));
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial Polygon for its Geospatial BoundingBox.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("box", "The BoundingBox.")]
        public static BoundingBox BoundingBox(this Polygon geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return null;
            }
            return BoundingBox(geospatial.Polygons.SelectMany(p => p.Points));
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial MultiPolygon for its Geospatial BoundingBox.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("box", "The BoundingBox.")]
        public static BoundingBox BoundingBox(this MultiPolygon geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return null;
            }
            return BoundingBox(geospatial.Polygons.SelectMany(p => p.Polygons.SelectMany(x => x.Points)));
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial Feature for its Geospatial BoundingBox.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("box", "The BoundingBox.")]
        public static BoundingBox BoundingBox(this Feature geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return null;
            }
            return BoundingBox(geospatial.Geometry as dynamic);
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial FeatureCollection for its Geospatial BoundingBox.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("box", "The BoundingBox.")]
        public static BoundingBox BoundingBox(this FeatureCollection geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return null;
            }
            BoundingBox box = BoundingBox(geospatial.Features.First().Geometry as dynamic);
            foreach (Feature feature in geospatial.Features)
                box += BoundingBox(feature.Geometry as dynamic);
            return box;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial GeometryCollection for its Geospatial BoundingBox.")]
        [Input("geospatial", "IGeospatial object to query.")]
        [Output("box", "The BoundingBox.")]
        public static BoundingBox BoundingBox(this GeometryCollection geospatial)
        {
            if (geospatial == null)
            {
                Base.Compute.RecordError("Cannot query a null geospatial object.");
                return null;
            }
            BoundingBox box = BoundingBox(geospatial.Geometries.First() as dynamic);
            foreach (IGeospatial feature in geospatial.Geometries)
                box += BoundingBox(feature as dynamic);
            return box;
        }

        /***************************************************/

        [Description("Method for querying an IGeospatial Point Collection for its Geospatial BoundingBox.")]
        [Input("geospatial", "IGeospatial collection to query.")]
        [Output("box", "The BoundingBox.")]
        public static BoundingBox BoundingBox(IEnumerable<Point> points)
        {
            Point min = new Point()
            {
                Latitude = points.Select(p => p.Latitude).Min(),
                Longitude = points.Select(p => p.Longitude).Min(),
                Altitude = points.Select(p => p.Altitude).Min()
            };
            Point max = new Point()
            {
                Latitude = points.Select(p => p.Latitude).Max(),
                Longitude = points.Select(p => p.Longitude).Max(),
                Altitude = points.Select(p => p.Altitude).Max()
            };
            return new BoundingBox()
            {
                Min = min,
                Max = max
            };
        }

        /***************************************************/
        /****           Private Fallback Method         ****/
        /***************************************************/
        private static BoundingBox BoundingBox(IGeospatial geospatial)
        {
            Base.Compute.RecordWarning($"BoundingBox could not be found for {geospatial.GetType()}.");
            return null;
        }
    }
}

