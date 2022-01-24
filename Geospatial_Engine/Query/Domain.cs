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
                    Base.Compute.RecordError($"Axis {axis} is not one of the axes associated with the Geospatial.BoundingBox only Latitude, Longitude or Altitude are permitted axes.");
                    return null;
            }
        }
    }
}

