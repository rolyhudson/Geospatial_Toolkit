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
    [Description("Class for representing geospatial points in The World Geodetic System (WGS 1984, EPSG:4326).")]
    public class Point : IGeospatial
    {
        [Description("The east–west position, in degrees of a point on the Earth's surface. Valid range is -180 to 180.")]
        public virtual double Longitude { get; set; } = 0;

        [Description("The north-south position, in degrees of a point on the Earth's surface. Valid range is -90 to 90.")]
        public virtual double Latitude { get; set; } = 0;

        [Description("The height above sea-level.")]
        public virtual double Altitude { get; set; } = 0;

        /***************************************************/
        /**** IComparable Interface                     ****/
        /***************************************************/

        public int CompareTo(Point other)
        {
            if (Longitude != other.Longitude)
                return Longitude.CompareTo(other.Longitude);
            else if (Latitude != other.Latitude)
                return Latitude.CompareTo(other.Latitude);
            else
                return Altitude.CompareTo(other.Altitude);
        }

        /***************************************************/

        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(Point) && this == ((Point)obj);
        }

        /***************************************************/

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Longitude.GetHashCode();
                hash = hash * 23 + Latitude.GetHashCode();
                hash = hash * 23 + Altitude.GetHashCode();
                return hash;
            }
        }
    }

    
}

