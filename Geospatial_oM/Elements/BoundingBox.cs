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

