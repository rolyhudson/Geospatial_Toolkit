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

using BH.oM.Geometry;
using GeoSp = BH.oM.Geospatial;
using CoordinateSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Geospatial
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        [Description("Interface method for converting an IGeospatial to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]        
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static IGeometry IToUTM(this GeoSp.IGeospatial geospatial, int gridZone = 0)
        {
            if(geospatial == null)
            {
                Reflection.Compute.RecordError("Cannot convert a null geospatial object");
                return null;
            }
            return ToUTM(geospatial as dynamic,  gridZone);
        }

        /***************************************************/

        [Description("Convert latitude and longitude to a Point in Universal Transverse Mercator coordinates.")]
        [Input("lat", "The latitude, in the range -90.0 to 90.0 with up to 7 decimal places.")]
        [Input("lon", "The longitude, in the range -180.0 to 180.0 with up to 7 decimal places.")]
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone.")]
        [Output("utmPoint", "Converted Node as a Point.")]

        public static Point ToUTM(double lat, double lon, int gridZone = 0)
        {
            if (lat < -90 || lat > 90 || lon < -180 || lon > 180)
            {
                Reflection.Compute.RecordError("One or more Point coordinates was outside the permitted ranges.");
                return null;
            }
            //EagerLoad sets which CoordinateSystems are calculated set all to false except UTM_MGRS
            EagerLoad el = new EagerLoad(false);
            el.UTM_MGRS = true;
            el.Extensions.MGRS = false;
            Coordinate c = new Coordinate(lat, lon, el);
            if (gridZone >= 1 && gridZone <= 60)
                c.Lock_UTM_MGRS_Zone(gridZone);
            Point utmPoint = Geometry.Create.Point(c.UTM.Easting, c.UTM.Northing, 0);
            return utmPoint;
        }

        /***************************************************/

        [Description("Method for converting an IGeospatial Point to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]        
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static Point ToUTM(this GeoSp.Point geospatial, int gridZone = 0)
        {
            if (geospatial == null)
            {
                Reflection.Compute.RecordError("Cannot convert a null geospatial object");
                return null;
            }
            Point utmPoint = ToUTM(geospatial.Latitude, geospatial.Longitude, gridZone);
            utmPoint.Z = geospatial.Altitude;
            return utmPoint;
        }

        /***************************************************/

        [Description("Method for converting an IGeospatial MultiPoint to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]        
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static CompositeGeometry ToUTM(this GeoSp.MultiPoint geospatial, int gridZone = 0)
        {
            if (geospatial == null)
            {
                Reflection.Compute.RecordError("Cannot convert a null geospatial object");
                return null;
            }
            CompositeGeometry composite = new CompositeGeometry();
            //if the zone has not been set
            if (gridZone == 0)
                gridZone = geospatial.UTMZone();
            ConcurrentBag<Point> points = new ConcurrentBag<Point>();
            Parallel.ForEach(geospatial.Points, n =>
            {
                Point utmPoint = ToUTM(n,  gridZone);
                if (utmPoint != null)
                    points.Add(utmPoint);
            }
            );
            composite.Elements.AddRange(points);
            return composite;
        }

        /***************************************************/

        [Description("Method for converting an IGeospatial LineString to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]        
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static Polyline ToUTM(this GeoSp.LineString geospatial, int gridZone = 0)
        {
            if (geospatial == null)
            {
                Reflection.Compute.RecordError("Cannot convert a null geospatial object");
                return null;
            }
            //if the zone has not been set
            if (gridZone == 0)
                gridZone = geospatial.UTMZone();
            //dictionary to ensure node order is maintained
            ConcurrentDictionary<int, Point> pointDict = new ConcurrentDictionary<int, Point>();
            Parallel.For(0, geospatial.Points.Count, n =>
            {
                Point utmPoint = ToUTM(geospatial.Points[n],  gridZone);
                if(utmPoint != null)
                    pointDict.TryAdd(n, utmPoint);
            }
            );
            List<Point> points = pointDict.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            Polyline utmPolyline = BH.Engine.Geometry.Create.Polyline(points);
            return utmPolyline;
        }

        /***************************************************/

        [Description("Method for converting an IGeospatial MultiLineString to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]       
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static CompositeGeometry ToUTM(this GeoSp.MultiLineString geospatial, int gridZone = 0)
        {
            if (geospatial == null)
            {
                Reflection.Compute.RecordError("Cannot convert a null geospatial object");
                return null;
            }
            //if the zone has not been set
            if (gridZone == 0)
                gridZone = geospatial.UTMZone();
            CompositeGeometry composite = new CompositeGeometry();
            ConcurrentBag<Polyline> polylines = new ConcurrentBag<Polyline>();
            Parallel.ForEach(geospatial.LineStrings, n =>
            {
                polylines.Add(ToUTM(n,  gridZone));
            }
            );

            composite.Elements.AddRange(polylines);
            return composite;
        }

        /**************************************************/

        [Description("Method for converting an IGeospatial Polygon to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]        
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static CompositeGeometry ToUTM(this GeoSp.Polygon geospatial, int gridZone = 0)
        {
            if (geospatial == null)
            {
                Reflection.Compute.RecordError("Cannot convert a null geospatial object");
                return null;
            }
            //if the zone has not been set
            if (gridZone == 0)
                gridZone = geospatial.UTMZone();
            CompositeGeometry composite = new CompositeGeometry();
            //dictionary to ensure node order is maintained
            ConcurrentDictionary<int, Polyline> polyDict = new ConcurrentDictionary<int, Polyline>();
            
            Parallel.For(0, geospatial.Polygons.Count, n =>
            {
                polyDict.TryAdd(n, ToUTM(geospatial.Polygons[n],  gridZone));
            }
            );
            List<Polyline> polylines = polyDict.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            composite.Elements.AddRange(polylines);
            return composite;
        }

        /**************************************************/

        [Description("Method for converting an IGeospatial MultiPolygon to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]        
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static CompositeGeometry ToUTM(this GeoSp.MultiPolygon geospatial, int gridZone = 0)
        {
            if (geospatial == null)
            {
                Reflection.Compute.RecordError("Cannot convert a null geospatial object");
                return null;
            }
            //if the zone has not been set
            if (gridZone == 0)
                gridZone = geospatial.UTMZone();
            CompositeGeometry composite = new CompositeGeometry();
            //dictionary to ensure node order is maintained
            ConcurrentDictionary<int, CompositeGeometry> polyDict = new ConcurrentDictionary<int, CompositeGeometry>();

            Parallel.For(0, geospatial.Polygons.Count, n =>
            {
                polyDict.TryAdd(n, ToUTM(geospatial.Polygons[n],  gridZone));
            }
            );
            
            List<CompositeGeometry> polylines = polyDict.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            composite.Elements.AddRange(polylines);
            return composite;
        }

        /**************************************************/

        [Description("Method for converting an IGeospatial BoundingBox to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]        
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static BoundingBox ToUTM(this GeoSp.BoundingBox geospatial, int gridZone = 0)
        {
            if (geospatial == null)
            {
                Reflection.Compute.RecordError("Cannot convert a null geospatial object");
                return null;
            }
            //if the zone has not been set
            if (gridZone == 0)
                gridZone = geospatial.UTMZone();

            BoundingBox boundingBox = new BoundingBox();
            boundingBox.Max = ToUTM(geospatial.Max,  gridZone);
            boundingBox.Min = ToUTM(geospatial.Min,  gridZone);
            return boundingBox;
        }

        /**************************************************/

        [Description("Method for converting an IGeospatial Feature to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]     
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static IGeometry ToUTM(this GeoSp.Feature geospatial, int gridZone = 0)
        {
            if (geospatial == null)
            {
                Reflection.Compute.RecordError("Cannot convert a null geospatial object");
                return null;
            }
            //if the zone has not been set
            if (gridZone == 0)
                gridZone = geospatial.UTMZone();
            return ToUTM(geospatial.Geometry as dynamic,  gridZone);
        }

        /**************************************************/

        [Description("Method for converting an IGeospatial FeatureCollection to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]        
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static CompositeGeometry ToUTM(this GeoSp.FeatureCollection geospatial, int gridZone = 0)
        {
            if (geospatial == null)
            {
                Reflection.Compute.RecordError("Cannot convert a null geospatial object");
                return null;
            }
            //if the zone has not been set
            if (gridZone == 0)
                gridZone = geospatial.UTMZone();
            CompositeGeometry composite = new CompositeGeometry();
            foreach (GeoSp.Feature f in geospatial.Features)
                composite.Elements.Add(ToUTM(f,  gridZone));
            return composite;
        }

        /**************************************************/

        [Description("Method for converting an IGeospatial GeometryCollection to Geometry in the Universal Transverse Mercator Projection.")]
        [Input("geospatial", "IGeospatial object to convert.")]        
        [Input("gridZone", "Optional Universal Transverse Mercator zone to allow locking conversion to a single zone. Permitted range is 1 - 60. " +
            "If set to zero the UTM zone will be automatically calculated, if objects are detected that span multiple zones the average zone is used. Default value is 0.")]
        [Output("geometry", "The converted geometry. The resulting geometry may be far from the model origin. Use fit view to locate the results.")]
        public static CompositeGeometry ToUTM(this GeoSp.GeometryCollection geospatial, int gridZone = 0)
        {
            if (geospatial == null)
            {
                Reflection.Compute.RecordError("Cannot convert a null geospatial object");
                return null;
            }
            //if the zone has not been set
            if (gridZone == 0)
                gridZone = geospatial.UTMZone();
            CompositeGeometry composite = new CompositeGeometry();
            foreach (GeoSp.IGeospatial f in geospatial.Geometries)
                composite.Elements.Add(ToUTM(f as dynamic,  gridZone));
            return composite;
        }

        /***************************************************/
        /****           Private Fallback Method         ****/
        /***************************************************/

        private static IGeometry ToUTM(GeoSp.IGeospatial geospatial, int gridZone = 0)
        {
            Reflection.Compute.RecordError($"Unable to convert {geospatial.GetType()} to Universal Transverse Mercator Coordinates.");
            return null;
        }
    }
}

